import {
    Card,
    CardHeader,
    CardPreview,
    CardFooter,
    Text,
    Body1,
    Button,
    Badge,
    RatingDisplay,
    Caption1,
    makeStyles,
    tokens
} from '@fluentui/react-components';
import { FaGithub, FaLinkedin } from 'react-icons/fa';

const useStyles = makeStyles({
    caption: {
        color: tokens.colorNeutralForeground3,
    },
});

export default function ProjectCard({ project }) {
    const { name, summary, technologies = [], image, repoUrl, liveUrl, source, rating } = project;
    const styles = useStyles();

    return (
        <Card size='small'>
            {image ? (
                <CardPreview>
                    <img src={image} alt={name} style={{ width: '100%', height: 140, objectFit: 'cover' }} />
                </CardPreview>
            ) : null}

            <CardHeader 
                header={<Text weight="semibold">{name}</Text>}
                description={
                    <Caption1 className={styles.caption}>
                        {summary}
                    </Caption1>
                }   
            />

            <div style={{ display: 'flex', gap: 6, flexWrap: 'wrap', marginTop: 8 }}>
                {technologies.slice(0, 4).map(t => <Badge key={t} appearance="outline">{t}</Badge>)}
                {source ? <Badge appearance="tint">{source}</Badge> : null}
            </div>
            
            <CardFooter style={{justifyContent: 'space-between'}}>
                <RatingDisplay value={rating}></RatingDisplay>
                <div style={{ alignSelf: 'end' }}>
                    {repoUrl ? (
                        <Button as="a" href={repoUrl} target="_blank" icon={<FaGithub />} />
                    ) : null}
                </div>
            </CardFooter>
        </Card>
    );
}
