import i18n from 'i18next';
import {initReactI18next} from 'react-i18next';
import HttpBackend from 'i18next-http-backend';
import LanguageDetector from 'i18next-browser-languagedetector';

i18n
    .use(HttpBackend)
    .use(LanguageDetector)
    .use(initReactI18next)
    .init({
        fallbackLng: 'pt-BR',
        debug: import.meta.env.DEV,
        interpolation: {
            escapeValue: false,
        },
        detection: {
            order: ['cookie', 'localStorage', 'navigator', 'htmlTag'],
            caches: ['cookie'],
        }
    });

export default i18n;