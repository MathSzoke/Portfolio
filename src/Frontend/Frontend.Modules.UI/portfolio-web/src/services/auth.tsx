import React, { createContext, useCallback, useContext, useEffect, useMemo, useState } from 'react';
import {jwtDecode} from "jwt-decode";
import getApiClient from './apiClient';

type SocialPhoto = { provider: string; userPhotoUrl: string };
type UserProfile = {
    id: string;
    name: string;
    email?: string;
    picture?: string;
    roles?: string[];
    socialPhotos?: SocialPhoto[];
    uploadedPhotos?: string[];
};

type AuthContextType = {
    token: string | null;
    userInfo: UserProfile | null;
    mathInfo: UserProfile | null;
    isLoading: boolean;
    login: (loginResponse: any) => void;
    logout: () => void;
    refreshToken: () => Promise<string>;
    isRole: (roles: string[]) => boolean;
    refreshUserProfile: () => Promise<void>;
    isAuthenticated: boolean;
};

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [token, setToken] = useState<string | null>(localStorage.getItem('authToken'));
    const [userInfo, setUserInfo] = useState<UserProfile | null>(null);
    const [mathInfo, setMathInfo] = useState<UserProfile | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    const login = useCallback((loginResponse: any) => {
        const newAccess = loginResponse.token || loginResponse.accessToken || loginResponse;

        if (newAccess) {
            localStorage.setItem('authToken', newAccess);
            localStorage.setItem('refreshToken', loginResponse.refreshToken);
            setToken(newAccess);
        }
    }, []);

    const logout = useCallback(async () => {
        try {
            const api = getApiClient();
            await api.post('/api/v1/auth/logout');
        } catch (_) { }
        localStorage.removeItem('authToken');
        setToken(null);
        setUserInfo(null);
    }, []);

    // === Auto-refresh do token ===
    const refreshToken = useCallback(async () => {
        try {
            const api = getApiClient();
            const refreshed = await api.post('/api/v1/auth/refresh',
                { refreshTokenStorage: localStorage.getItem('refreshToken') },
                { skipAuth: true }
            );
            const newToken = refreshed.accessToken;
            const newRefresh = refreshed.refreshToken;

            if (!newToken || !newRefresh) throw new Error('Invalid refresh response');

            localStorage.setItem('authToken', newToken);
            localStorage.setItem('refreshToken', newRefresh);

            setToken(newToken);
            return newToken;

        } catch (err) {
            await logout();
            throw err;
        }
    }, [logout]);

    useEffect(() => {
        if (!token) return;

        const decoded: any = jwtDecode(token);
        const expSeconds = decoded.exp ? decoded.exp * 1000 : Date.now() + 14 * 60 * 1000;
        const now = Date.now();
        let timeout = expSeconds - now - 30 * 1000;

        if (timeout < 0) timeout = 0;

        const handle = setTimeout(async function refreshLoop() {
            try {
                await refreshToken();
                const newToken = localStorage.getItem('authToken');
                if (!newToken) throw new Error("Refresh failed to return a new token.");

                const decodedNew: any = jwtDecode(newToken);
                const nextExp = decodedNew.exp ? decodedNew.exp * 1000 : Date.now() + 14 * 60 * 1000;
                const delay = nextExp - Date.now() - 30 * 1000;
                setTimeout(refreshLoop, delay > 0 ? delay : 0);
            } catch (err) {
                console.error('[Auth] Falha ao auto-refresh do token:', err);
            }
        }, timeout);

        return () => clearTimeout(handle);
    }, [token, refreshToken, logout]);

    const fetchUserProfile = useCallback(async (t: string) => {
        try {
            const decoded: any = jwtDecode(t);
            const roleClaim =
                decoded.roles ?? decoded.role ?? decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
            const profile: UserProfile = {
                id: decoded.sub || decoded.userId || '',
                name: decoded.name || '',
                email: decoded.email || '',
                picture: decoded.picture || '',
                roles: Array.isArray(roleClaim) ? roleClaim : roleClaim ? [roleClaim] : [],
            };
            const api = getApiClient(refreshToken);
            const userApiResp = await api.get('/api/v1/User');
            const d = userApiResp ?? {};
            profile.socialPhotos = d.socialPhotos || [];
            profile.uploadedPhotos = d.uploadedPhotos || [];
            if (d.avatarUrl) profile.picture = d.avatarUrl;
            if (d.fullName) profile.name = d.fullName;
            setUserInfo(profile);
        } catch (e) {
            setUserInfo(null);
        }
    }, [refreshToken]);

    useEffect(() => {
        async function fetchMathInfo() {
            try {
                const api = getApiClient(refreshToken);
                const result = await api.get('/api/v1/User/matheusszoke@gmail.com');
                const d = result ?? {};
                if (d) {
                    setMathInfo({
                        id: d.id,
                        name: d.fullName,
                        email: d.email,
                        picture: d.avatarUrl
                    });
                }
            } catch (_) { }
        }
        fetchMathInfo();
    }, [refreshToken]);

    const refreshUserProfile = useCallback(async () => {
        if (token) await fetchUserProfile(token);
    }, [token, fetchUserProfile]);

    useEffect(() => {
        let cancelled = false;
        if (token) {
            setIsLoading(true);
            fetchUserProfile(token).finally(() => { if (!cancelled) setIsLoading(false); });
        } else {
            setUserInfo(null);
            setIsLoading(false);
        }
        return () => { cancelled = true; };
    }, [token, fetchUserProfile]);

    const isRole = (requiredRoles: string[]) => {
        if (!userInfo?.roles || !Array.isArray(requiredRoles)) return false;
        return requiredRoles.some(r => userInfo.roles?.includes(r));
    };

    const value = useMemo(() => ({
        token,
        userInfo,
        mathInfo,
        isLoading,
        login,
        logout,
        refreshToken,
        isRole,
        isAuthenticated: !!token,
        refreshUserProfile
    }), [token, userInfo, mathInfo, isLoading, login, logout, refreshToken, isRole, refreshUserProfile]);

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error('useAuth must be used within AuthProvider');
    return ctx;
};

