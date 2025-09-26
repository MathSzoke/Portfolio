import { useState, useCallback, useEffect } from 'react';
import { Button, Toolbar, ToolbarButton, makeStyles, mergeClasses } from '@fluentui/react-components';
import { Settings24Regular } from '@fluentui/react-icons';
import { useTranslation } from 'react-i18next';
import SettingsModal from '../Settings/General/SettingsModal';
import AuthModal from '../auth/AuthModal.jsx';
import UserSettingsModal from '../Settings/User/UserSettingsModal.jsx';
import UserPersonaButton from '../header/UserPersonaButton.jsx';
import { useAuth } from '../../services/auth';

const useStyles = makeStyles({
    wrapper: {
        position: 'fixed',
        width: '100%',
        top: 0,
        zIndex: 20,
        backgroundColor: 'transparent',
        borderBottom: '1px solid transparent',
        backdropFilter: 'none',
        WebkitBackdropFilter: 'none',
        transition: 'background-color 150ms ease, backdrop-filter 150ms ease, border-color 150ms ease, box-shadow 150ms ease'
    },
    scrolled: {
        backgroundColor: 'color-mix(in srgb, var(--colorNeutralBackground1) 95%, transparent)'
    },
    bar: {
        maxWidth: 1200,
        margin: '0 auto',
        display: 'grid',
        gridTemplateColumns: '1fr auto 1fr',
        alignItems: 'center',
        padding: '12px 16px',
        gap: 12
    },
    center: { justifySelf: 'center' },
    right: {
        justifySelf: 'end',
        display: 'flex',
        gap: '3em',
        alignItems: 'center'
    }
});

export default function HeaderSection() {
    const s = useStyles();
    const { t } = useTranslation();
    const [openSettings, setOpenSettings] = useState(false);
    const [scrolled, setScrolled] = useState(false);

    const { isAuthenticated } = useAuth();

    const scrollToSection = useCallback((id) => {
        const el = document.getElementById(id);
        if (el && el.scrollIntoView) {
            el.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    }, []);

    useEffect(() => {
        const onScroll = () => setScrolled(window.scrollY > 8);
        onScroll();
        window.addEventListener('scroll', onScroll, { passive: true });
        return () => window.removeEventListener('scroll', onScroll);
    }, []);

    return (
        <>
            <header className={mergeClasses(s.wrapper, scrolled && s.scrolled)}>
                <div className={s.bar}>
                    <div />
                    <div className={s.center}>
                        <Toolbar size="small" aria-label="Navigation">
                            <ToolbarButton appearance="subtle" onClick={() => scrollToSection('hero')}>{t('header.nav.hero')}</ToolbarButton>
                            <ToolbarButton appearance="subtle" onClick={() => scrollToSection('about')}>{t('header.nav.about')}</ToolbarButton>
                            <ToolbarButton appearance="subtle" onClick={() => scrollToSection('projects')}>{t('header.nav.projects')}</ToolbarButton>
                        </Toolbar>
                    </div>
                    <div className={s.right}>
                        <Button appearance="subtle" aria-label={t('header.buttons.settings')} icon={<Settings24Regular />} onClick={() => setOpenSettings(true)} />
                        {isAuthenticated ? (
                            <UserPersonaButton />
                        ) : (
                            <Button appearance="secondary" onClick={() => window.dispatchEvent(new CustomEvent('open-login'))}>
                                {t('header.buttons.login')}
                            </Button>
                        )}
                    </div>
                </div>
            </header>
            <SettingsModal isOpen={openSettings} onClose={() => setOpenSettings(false)} />
            <UserSettingsModal />
            <AuthModal />
        </>
    );
}
