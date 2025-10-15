import {
    Text,
    Title3,
    Badge,
    makeStyles,
    tokens,
    Button,
    List,
    Card,
    CardHeader,
    mergeClasses
} from '@fluentui/react-components';
import { useTranslation } from 'react-i18next';
import { DocumentPdfFilled } from '@fluentui/react-icons';
import { Carousel, CarouselNav, CarouselNavButton, CarouselNavContainer, CarouselSlider, CarouselViewport, CarouselCard } from '@fluentui/react-components';
import { useState } from 'react';

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
    sub: {
        color: tokens.colorNeutralForeground3,
        whiteSpace: 'pre-line'
    },
    button: {
        width: 'max-content'
    },
    card: {
        minWidth: '300px',
        maxWidth: '350px',
        display: 'flex',
        flexDirection: 'column',
        gap: '6px',
        margin: '0 0.5em',
        position: 'relative',
        transition: 'box-shadow 0.3s ease, transform 0.3s ease',
        boxShadow: tokens.shadow4,
        cursor: 'pointer',
        background: tokens.colorNeutralBackground1,
        '@media (max-width: 768px)': {
            margin: 0,
        },
    },
    cardHovered: {
        boxShadow: tokens.shadow8,
        transform: 'scale(1.07)',
        zIndex: 2
    },
    logo: {
        width: '40px',
        height: '40px',
        borderRadius: '8px',
        objectFit: 'contain',
        backgroundColor: tokens.colorNeutralBackground3
    },
    period: {
        fontSize: '12px',
        color: tokens.colorNeutralForeground3
    },
    description: {
        width: '100%',
        overflow: 'hidden',
        background: 'rgba(30,30,30,0.97)',
        color: tokens.colorNeutralForegroundOnBrand,
        borderRadius: tokens.borderRadiusMedium,
        fontSize: '1rem',
        boxShadow: tokens.shadow8,
        opacity: 0,
        maxHeight: 0,
        transform: 'translateY(-10px)',
        transition: 'opacity 0.3s ease, max-height 0.3s ease, transform 0.3s ease',
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'flex-start',
    },
    descriptionVisible: {
        opacity: 1,
        maxHeight: 400,
        padding: '12px 0',
        transform: 'translateY(0)',
    },
    techs: {
        display: 'flex',
        gap: '6px',
        flexWrap: 'wrap',
        marginTop: '0',
        opacity: 0,
        maxHeight: 0,
        overflow: 'hidden',
        transform: 'translateY(-10px)',
        transition: 'opacity 0.3s ease, max-height 0.3s ease, transform 0.3s ease',
    },
    techsVisible: {
        opacity: 1,
        maxHeight: 200,
        marginTop: '8px',
        transform: 'translateY(0)',
    },
    languagesItem: {
        padding: '0 0 0 30px',
        listStyleType: 'inherit'
    },
    skillsTechs: {
        display: 'inline-flex',
        flexWrap: 'wrap',
        gap: '5px'
    }
});

const getAnnouncement = (index, totalSlides) => `Carrossel: slide ${index + 1} de ${totalSlides}`;

export default function AboutSection() {
    const s = useStyles();
    const { t } = useTranslation();
    const sections = t('about.sections', { returnObjects: true });
    const experiences = t('about.sections.experiences.items', { returnObjects: true });
    const [hovered, setHovered] = useState(null);

    return (
        <div className={s.root}>
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
                <Text weight="semibold">{sections.experiences.title}</Text>
                <Text className={s.sub}>{sections.experiences.text}</Text>

                <Carousel align="center" whitespace={false} announcement={getAnnouncement} draggable>
                    <CarouselViewport>
                        <CarouselSlider
                            cardFocus
                            aria-label="Use as setas para navegar entre as experiências"
                            style={{ display: 'flex', gap: '16px', padding: '1em' }}
                        >
                            {experiences.map((exp, i) => {
                                const isHovered = hovered === i;
                                return (
                                    <CarouselCard autoSize key={i}>
                                        <Card
                                            className={mergeClasses(s.card, isHovered && s.cardHovered)}
                                            onMouseEnter={() => setHovered(i)}
                                            onMouseLeave={() => setHovered(null)}
                                            tabIndex={0}
                                            aria-label={exp.company}
                                            style={{ position: 'relative' }}
                                        >
                                            <CardHeader
                                                image={<img src={exp.logo} alt={exp.company} className={s.logo} />}
                                                header={<Text weight="semibold">{exp.role}</Text>}
                                                description={<Text>{exp.company}</Text>}
                                            />
                                            <Text className={s.period}>{exp.period}</Text>
                                            <Text className={s.sub}>{exp.location}</Text>

                                            <div className={mergeClasses(s.description, isHovered && s.descriptionVisible)}>
                                                <Text weight="semibold" style={{ padding: 8, marginBottom: 8 }}>
                                                    {t('about.sections.experiences.descriptionTitle', 'Main activities')}
                                                </Text>
                                                <ul style={{ padding: '0 40px 10px', margin: 0 }}>
                                                    {exp.description.map((desc, idx) => (
                                                        <li key={idx}><Text size={200}>{desc}</Text></li>
                                                    ))}
                                                </ul>
                                            </div>

                                            <div className={mergeClasses(s.techs, isHovered && s.techsVisible)}>
                                                {exp.techs.map((tech, idx) => (
                                                    <Badge key={idx} appearance="outline">{tech}</Badge>
                                                ))}
                                            </div>
                                        </Card>
                                    </CarouselCard>
                                );
                            })}
                        </CarouselSlider>
                    </CarouselViewport>
                    <CarouselNavContainer
                        layout="inline"
                        autoplayTooltip={{ content: 'Autoplay', relationship: 'label' }}
                        nextTooltip={{ content: 'Próximo', relationship: 'label' }}
                        prevTooltip={{ content: 'Anterior', relationship: 'label' }}
                    >
                        <CarouselNav>
                            {(index) => (
                                <CarouselNavButton index={index} aria-label={`Ir para o slide ${index + 1}`} />
                            )}
                        </CarouselNav>
                    </CarouselNavContainer>
                </Carousel>
            </div>

            <div className={s.section}>
                <Text weight="semibold">{sections.skills.title}</Text>
                <div className={s.skillsTechs}>
                    {
                        sections.skills.techs.map((tech) => (
                            <Badge key={tech} appearance="tint" shape="rounded">{tech}</Badge>
                        )
                    )}
                </div>
            </div>

            <div className={s.section}>
                <Text weight="semibold">{sections.languages.title}</Text>
                <List className={s.languagesItem}>
                    <li>
                        <Text className={s.sub}>{sections.languages.text.enUs}</Text>
                    </li>
                    <li>
                        <Text className={s.sub}>{sections.languages.text.ptBr}</Text>
                    </li>
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
        </div>
    );
}
