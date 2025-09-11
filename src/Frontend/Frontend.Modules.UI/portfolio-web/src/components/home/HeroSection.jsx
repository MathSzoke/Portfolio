import { Button, Text, Title3, tokens, makeStyles } from '@fluentui/react-components';
import { Chat24Regular } from '@fluentui/react-icons';
import { useTranslation } from 'react-i18next';

const useStyles = makeStyles({
    root: {
        display: 'grid',
        gap: 16,
    },
    sub: { color: tokens.colorNeutralForeground3 },
});

export default function HeroSection({ onSeeProjects }) {
    const s = useStyles();
    const { t } = useTranslation();

    return (
        <section id={"hero"} className={s.root}>
            <Title3>{t('hero.title')}</Title3>
            <Text className={s.sub}>{t('hero.subtitle')}</Text>
        </section>
    );
}
