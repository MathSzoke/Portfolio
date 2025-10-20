import React, { useEffect } from 'react';
import { Outlet, useNavigate, useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { languages, getUrlCodeFromI18n, FALLBACK_URL_CODE } from './languageConfig.ts';

const validUrlCodes = languages.map(l => l.urlCode);

export default function LanguageLayout() {
    const { lang } = useParams();
    const navigate = useNavigate();
    const { i18n } = useTranslation();

    const urlCode = validUrlCodes.includes((lang || '').toLowerCase()) ? (lang || '').toLowerCase() : FALLBACK_URL_CODE;
    const targetI18n = languages.find(l => l.urlCode === urlCode).i18nCode;

    useEffect(() => {
        if (i18n.resolvedLanguage !== targetI18n) i18n.changeLanguage(targetI18n);
        const canonical = getUrlCodeFromI18n(i18n.resolvedLanguage ?? i18n.language, urlCode);
        if (canonical !== urlCode) navigate(`/${canonical}`, { replace: true });
    }, [urlCode, targetI18n, i18n.resolvedLanguage, i18n.language, navigate]);

    return <Outlet />;
}