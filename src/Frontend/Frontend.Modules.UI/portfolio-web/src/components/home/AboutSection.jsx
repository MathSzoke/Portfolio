import { Text, Title3, Badge, makeStyles, tokens } from '@fluentui/react-components';

const useStyles = makeStyles({
    root: { maxWidth: 1200 },
    techs: { display: 'flex', gap: '5px', flexWrap: 'wrap', marginTop: 12 },
    sub: { color: tokens.colorNeutralForeground3 }
});

export default function AboutSection() {
    const s = useStyles();
    const techs = ['.NET 9', 'React', 'PostgreSQL', 'Docker', 'Python', 'Azure'];

    return (
        <section id={"about"} className={s.root}>
            <Title3>Sobre mim</Title3>
            <Text className={s.sub} style={{ display: 'block', marginTop: 8 }}>
                Sou engenheiro focado em .NET, arquitetura limpa e IA aplicada. Gosto de criar soluções de ponta a ponta.
            </Text>
            <div className={s.techs}>
                {techs.map(t => <Badge key={t} appearance="tint" shape="rounded">{t}</Badge>)}
            </div>
        </section>
    );
}
