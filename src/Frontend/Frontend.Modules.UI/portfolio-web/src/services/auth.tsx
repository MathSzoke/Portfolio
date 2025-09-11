import React, { createContext, useCallback, useContext, useEffect, useMemo, useState } from 'react';
import { jwtDecode } from 'jwt-decode';
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
    isLoading: boolean;
    login: (loginResponse: any) => void;
    logout: () => void;
    refreshToken: (options?: { silent?: boolean }) => Promise<string>;
    isRole: (roles: string[]) => boolean;
    refreshUserProfile: () => Promise<void>;
};

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [token, setToken] = useState<string | null>(localStorage.getItem('authToken'));
    const [userInfo, setUserInfo] = useState<UserProfile | null>(null);
    const [mathInfo, setMathInfo] = useState<UserProfile | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    const login = useCallback((loginResponse: any) => {
        const newToken = loginResponse.token || loginResponse.accessToken || loginResponse;
        localStorage.setItem('authToken', newToken);
        setToken(newToken);
    }, []);

    const logout = useCallback(async () => {
        const api = getApiClient();
        await api.post('/api/v1/auth/logout');
        localStorage.removeItem('authToken');
        setToken(null);
        setUserInfo(null);
    }, []);

    const refreshToken = useCallback(async ({ silent = false } = {}) => {
        try {
            const api = getApiClient();
            const refreshed = await api.post('/api/v1/auth/refresh');
            localStorage.setItem('authToken', refreshed.token || refreshed.accessToken);
            if (!silent) login(refreshed);
            return refreshed.token || refreshed.accessToken;
        } catch (err) {
            logout();
            throw err;
        }
    }, [login, logout]);

    const fetchUserProfile = useCallback(async (token: string) => {
        const decoded = jwtDecode<any>(token);
        const profile: UserProfile = {
            id: decoded.sub || decoded.userId || '',
            name: decoded.name || '',
            email: decoded.email || '',
            picture: decoded.picture || '',
            roles: Array.isArray(decoded.roles)
                ? decoded.roles
                : decoded.roles
                    ? [decoded.roles]
                    : Array.isArray(decoded.role)
                        ? decoded.role
                        : decoded.role
                            ? [decoded.role]
                            : [],
        };
        const api = getApiClient();
        const userApiResp = await api.get('/api/v1/User');
        if (userApiResp) {
            profile.socialPhotos = userApiResp.socialPhotos || [];
            profile.uploadedPhotos = userApiResp.uploadedPhotos || [];
            if (userApiResp.avatarUrl) profile.picture = userApiResp.avatarUrl;
            if (userApiResp.fullName) profile.name = userApiResp.fullName;
        }
        setUserInfo(profile);
    }, []);

    useEffect(() => {
        async function fetchMathInfo() {
            const api = getApiClient();
            const result = await api.get('/api/v1/User/matheusszoke@gmail.com');
            if (result) {
                setMathInfo({
                    id: result.id,
                    name: result.fullName,
                    email: result.email,
                    picture: result.avatarUrl
                });
            }
        }
        fetchMathInfo();
    }, []);

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
    }), [token, userInfo, isLoading, login, logout, refreshToken, isRole, refreshUserProfile]);

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error('useAuth must be used within AuthProvider');
    return ctx;
};
