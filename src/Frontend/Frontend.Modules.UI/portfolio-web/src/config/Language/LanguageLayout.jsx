import React, {useEffect} from 'react';
import {Outlet, useParams} from 'react-router-dom';
import {useTranslation} from 'react-i18next';

const langUrlMap = {
    'br': 'pt-BR',
    'us': 'en-US',
};
const supportedLanguages = Object.values(langUrlMap);

const LanguageLayout = () => {
    const {lang: urlLang} = useParams();
    const {i18n} = useTranslation();
    const i18nLang = langUrlMap[urlLang] || 'pt-BR';

    useEffect(() => {
        if (supportedLanguages.includes(i18nLang) && i18n.resolvedLanguage !== i18nLang) {
            i18n.changeLanguage(i18nLang);
        }
    }, [i18nLang, i18n]);

    return (
        <Outlet/>
    );
};

export default LanguageLayout;