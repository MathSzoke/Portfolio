import {
    Card,
    CardHeader,
    CardPreview,
    CardFooter,
    Text,
    Button,
    Badge,
    RatingDisplay,
    Rating,
    Caption1,
    makeStyles,
    tokens
} from '@fluentui/react-components';
import { FaGithub, FaEdit } from 'react-icons/fa';
import { useState } from 'react';
import { Dismiss24Regular } from '@fluentui/react-icons';
import getApiClient from '../../services/apiClient';

const useStyles = makeStyles({
    caption: {
        color: tokens.colorNeutralForeground3,
    },
    dangerButton: {
        backgroundColor: tokens.colorStatusDangerBackground1,
        color: tokens.colorStatusDangerForeground1
    }
});

export default function ProjectCard({ project, superAdmin, userInfo, onClickEditable, onClickDeletable }) {
    const { id, name, summary, technologies = [], thumbnailUrl, repoName, rating, ratingCount } = project;
    const styles = useStyles();
    const [isEditing, setIsEditing] = useState(false);
    const [currentRating, setCurrentRating] = useState(rating);
    const [currentCount, setCurrentCount] = useState(ratingCount);
    const api = getApiClient();

    async function handleRate(value) {
        if (userInfo === null) {
            window.dispatchEvent(new Event('open-login'));
            setIsEditing(false);
            return;
        }

        try {
            const res = await api.post(`/api/v1/Projects/${id}/rate`, { Rating: value });
            setCurrentRating(res.rating);
            setCurrentCount(res.ratingCount);
        } catch (err) {
            console.error("Erro ao salvar avaliação:", err);
        }
    }

    return (
        <Card size='small'>
            {thumbnailUrl ? (
                <CardPreview>
                    <img src={thumbnailUrl} alt={name} style={{ width: '100%', height: 140, objectFit: 'cover' }} />
                </CardPreview>
            ) : null}

            <CardHeader
                header={<Text weight="semibold">{name}</Text>}
                description={<Caption1 className={styles.caption}>{summary}</Caption1>}
            />

            <div style={{ display: 'flex', gap: 6, flexWrap: 'wrap', marginTop: 8 }}>
                {technologies.slice(0, 4).map(t => (
                    <Badge size='small' key={t} appearance="outline">{t}</Badge>
                ))}
            </div>

            <CardFooter style={{ justifyContent: 'space-between' }}>
                {isEditing ? (
                    <Rating
                        step={0.5}
                        size='large'
                        style={{ alignItems: 'center' }}
                        value={currentRating}
                        onChange={(_, data) => handleRate(data.value)}
                        onMouseLeave={() => setIsEditing(false)}
                    />
                ) : (
                    <RatingDisplay
                        value={currentRating}
                        count={currentCount}
                        onMouseEnter={() => setIsEditing(true)}
                    />
                )}
                <div style={{ alignSelf: 'end' }}>
                    {repoName ? (
                        <Button as="a" href={`https://github.com/MathSzoke/${repoName}`} target="_blank" icon={<FaGithub />} />
                    ) : null}
                </div>
            </CardFooter>

            {superAdmin && (
                <CardFooter style={{ justifyContent: 'space-between' }}>
                    <Button appearance="outline" className={styles.dangerButton} icon={<Dismiss24Regular />} onClick={onClickDeletable}></Button>
                    <Button appearance="primary" icon={<FaEdit />} onClick={onClickEditable}></Button>
                </CardFooter>
            )}
        </Card>
    );
}
