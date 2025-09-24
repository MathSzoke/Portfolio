import { Button, Text, Title3, tokens, makeStyles } from '@fluentui/react-components';
import { useTranslation } from 'react-i18next';

const useStyles = makeStyles({
    root: {
        display: 'grid',
        gap: '16px',
    },
    sub: {
        color: tokens.colorNeutralForeground3,
        whiteSpace: 'pre-line'
    },
});

export default function HeroSection({ onSeeProjects }) {
    const s = useStyles();
    const { t } = useTranslation();

    const birthDate = new Date('2001-12-08');

    const age = (() => {
        const today = new Date();
        let years = today.getFullYear() - birthDate.getFullYear();
        const monthDiff = today.getMonth() - birthDate.getMonth();
        const dayDiff = today.getDate() - birthDate.getDate();
        if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) years--;
        return years;
    })();

    return (
        <section id={"hero"} className={s.root}>
            <Title3>{t('hero.title')}</Title3>
            <Text className={s.sub}>
                {t('hero.subtitle', { age })}
            </Text>
        </section>
    );
}
