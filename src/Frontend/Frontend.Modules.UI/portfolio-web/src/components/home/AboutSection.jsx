import {
    Text,
    Title3,
    Badge,
    makeStyles,
    tokens,
    Button,
    List,
    ListItem
} from '@fluentui/react-components';
import { useTranslation } from 'react-i18next';
import { DocumentPdfFilled } from '@fluentui/react-icons';

const useStyles = makeStyles({
    root: {
        maxWidth: '1200px',
        display: 'grid',
        gap: '24px'
    },
    section: {
        display: 'grid',
        gap: '8px'
    },
    techs: {
        display: 'flex',
        gap: '6px',
        flexWrap: 'wrap',
        marginTop: '8px',
        marginBottom: '8px'
    },
    sub: {
        color: tokens.colorNeutralForeground3,
        whiteSpace: 'pre-line'
    },
    button: {
        width: 'max-content'
    }
});

export default function AboutSection() {
    const s = useStyles();
    const { t } = useTranslation();

    const sections = t('about.sections', { returnObjects: true });

    return (
        <section id="about" className={s.root}>
            <Title3>{t('about.title')}</Title3>

            <div className={s.section}>
                <Text weight="semibold">{sections.summary.title}</Text>
                <Text className={s.sub}>{sections.summary.text}</Text>
            </div>

            <div className={s.section}>
                <Text weight="semibold">{sections.interests.title}</Text>
                <Text className={s.sub}>{sections.interests.text}</Text>
            </div>

            <div className={s.section}>
                <Text weight="semibold">{sections.skills.title}</Text>
                <div className={s.techs}>
                    {sections.skills.techs.map((tech) => (
                        <Badge key={tech} appearance="tint" shape="rounded">{tech}</Badge>
                    ))}
                </div>
            </div>

            <div className={s.section}>
                <Text weight="semibold">{sections.languages.title}</Text>
                <List>
                    <ListItem>
                        <Text className={s.sub}>{sections.languages.text.enUs}</Text>
                    </ListItem>
                    <ListItem>
                        <Text className={s.sub}>{sections.languages.text.ptBr}</Text>
                    </ListItem>
                </List>
            </div>

            <Button
                appearance="primary"
                icon={<DocumentPdfFilled />}
                className={s.button}
                as="a"
                href="/assets/cv.pdf"
                target="_blank"
            >
                {sections.cta.viewCurriculum}
            </Button>
        </section>
    );
}
