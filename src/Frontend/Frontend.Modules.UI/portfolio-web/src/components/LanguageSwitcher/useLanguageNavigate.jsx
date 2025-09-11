import {useTranslation} from 'react-i18next';
import {useLocation, useNavigate} from 'react-router-dom';
import {useCallback} from 'react';

const languages = [
    {urlCode: 'br', i18nCode: 'pt-BR', label: 'Português (BR)', countryCode: 'br'},
    {urlCode: 'us', i18nCode: 'en-US', label: 'English (US)', countryCode: 'us'},
];

export const useLanguageNavigate = () => {
    const {i18n} = useTranslation();
    const navigate = useNavigate();
    const location = useLocation();

    const getActiveUrlCode = useCallback(() => {
        const currentLanguageConfig = languages.find(l => l.i18nCode === i18n.resolvedLanguage);
        return currentLanguageConfig?.urlCode || 'br';
    }, [i18n.resolvedLanguage]);

    const navigateTo = useCallback((destination) => {
        const urlLangCode = getActiveUrlCode();
        const finalDestination = destination.startsWith('/') ? destination : `/${destination}`;
        navigate(`/${urlLangCode}${finalDestination === '/' ? '' : finalDestination}`);
    }, [getActiveUrlCode, navigate]);

    const changeLanguage = useCallback((newUrlCode) => {
        const pathParts = location.pathname.split('/').filter(p => p);

        if (pathParts.length > 0) {
            pathParts[0] = newUrlCode;
        } else {
            pathParts.push(newUrlCode);
        }

        const newPath = `/${pathParts.join('/')}`;
        navigate(newPath);
    }, [location.pathname, navigate]);

    return {navigateTo, changeLanguage, languages, i18n, getActiveUrlCode};
};