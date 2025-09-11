import i18n from '../config/i18n';

const getApiClient = (refreshTokenCallback?: () => Promise<any>) => {
    const backendUrl = import.meta.env.VITE_PORTFOLIO_API || import.meta.env.VITE_API_BASE_URL;
    if (!backendUrl) throw new Error("API base URL not set.");

    const request = async (method: string, endpoint: string, body: any = null) => {
        const makeFetch = async () => {
            const currentLang = i18n.resolvedLanguage || 'pt-BR';
            const token = localStorage.getItem('authToken');
            const headers = new Headers({
                'Content-Type': 'application/json',
                'Accept-Language': currentLang,
            });
            if (token) headers.append('Authorization', `Bearer ${token}`);
            const config: RequestInit = {
                method: method.toUpperCase(),
                headers,
                credentials: 'include',
            };
            if (body) config.body = JSON.stringify(body);
            return fetch(`${backendUrl}${endpoint}`, config);
        };

        let response = await makeFetch();

        if (response.status === 401 && typeof refreshTokenCallback === 'function') {
            try {
                await refreshTokenCallback();
                response = await makeFetch();
            } catch {
                throw new Error("Sessão expirada. Por favor, faça login novamente.");
            }
        }

        if (!response.ok) {
            const errorData = await response.json().catch(() => ({}));
            const error: any = new Error(errorData.detail || `HTTP error! status: ${response.status}`);
            error.response = response;
            error.data = errorData;
            throw error;
        }

        if (response.status === 204) return null;
        return response.json();
    };

    return {
        get: (endpoint: string) => request('GET', endpoint),
        post: (endpoint: string, body?: any) => request('POST', endpoint, body),
        put: (endpoint: string, body?: any) => request('PUT', endpoint, body),
        delete: (endpoint: string, body?: any) => request('DELETE', endpoint, body),
    };
};

export default getApiClient;
