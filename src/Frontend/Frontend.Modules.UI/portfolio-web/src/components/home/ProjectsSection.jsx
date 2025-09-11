import { useEffect, useMemo, useState } from 'react';
import { Spinner, makeStyles, Toaster, useToastController, Title3 } from '@fluentui/react-components';
import ProjectCard from '../projects/ProjectCard.jsx';
import { getAggregatedProjects, reorderProjects } from '../../services/projects.js';
import { useTranslation } from 'react-i18next';
import { useAuth } from '../../services/auth.tsx';
import { DndContext, closestCenter, PointerSensor, useSensor, useSensors } from '@dnd-kit/core';
import { SortableContext, rectSortingStrategy, arrayMove } from '@dnd-kit/sortable';
import SortableItem from './SortableItem.jsx';

const useStyles = makeStyles({
    root: { maxWidth: 1200 },
    grid: {
        display: 'grid',
        gap: '32px',
        gridTemplateColumns: 'repeat(auto-fill,minmax(280px,1fr))',
        marginTop: '2em'
    }
});

export default function ProjectsSection() {
    const s = useStyles();
    const { t } = useTranslation();
    const { user } = useAuth();
    const canReorder = Array.isArray(user?.roles) && user.roles.includes('SuperAdmin');
    const [loading, setLoading] = useState(true);
    const [projects, setProjects] = useState([]);
    const sensors = useSensors(useSensor(PointerSensor, { activationConstraint: { distance: 6 } }));
    const { dispatchToast } = useToastController();

    useEffect(() => {
        let alive = true;
        (async () => {
            setLoading(true);
            try {
                const fakeProjects = [
                    {
                        slug: 'portfolio',
                        name: 'Meu Portfólio',
                        summary: 'Aplicação pessoal para exibir projetos e experiências.',
                        technologies: ['React', 'FluentUI', 'Vite'],
                        image: 'https://picsum.photos/400/200?random=1',
                        repoUrl: 'https://github.com/matheus/portfolio',
                        liveUrl: 'https://meuportfolio.dev',
                        source: 'aspire',
                        rating: 1
                    },
                    {
                        slug: 'erp-saas',
                        name: 'ERP SaaS',
                        summary: 'Plataforma ERP modular com multi-tenancy e dashboards interativos.',
                        technologies: ['.NET 9', 'PostgreSQL', 'Docker', 'Aspire'],
                        image: 'https://picsum.photos/400/200?random=2',
                        repoUrl: 'https://github.com/matheus/erp-saas',
                        liveUrl: 'https://erp-saas.dev',
                        source: 'aspire',
                        rating: 3.5
                    },
                    {
                        slug: 'wager-guard',
                        name: 'WagerGuard',
                        summary: 'Projeto de IA para bloquear sites de apostas online.',
                        technologies: ['Python', 'FastAPI', 'TensorFlow'],
                        image: 'https://picsum.photos/400/200?random=3',
                        repoUrl: 'https://github.com/matheus/wager-guard',
                        liveUrl: null,
                        source: 'external',
                        rating: 4
                    },
                    {
                        slug: 'bitern-ai',
                        name: 'Bitern AI',
                        summary: 'Assistente de IA integrado a sistemas SaaS corporativos.',
                        technologies: ['LangChain', 'OpenAI', 'Redis'],
                        image: 'https://picsum.photos/400/200?random=4',
                        repoUrl: 'https://github.com/matheus/bitern-ai',
                        liveUrl: 'https://bitern.ai',
                        source: 'external',
                        rating: 5
                    }
                ];
                const apiData = await getAggregatedProjects().catch(() => null);
                const data = Array.isArray(apiData) && apiData.length > 0 ? apiData : fakeProjects;
                if (alive) setProjects(data);
            } finally {
                if (alive) setLoading(false);
            }
        })();
        return () => { alive = false; };
    }, []);

    const items = useMemo(() => projects.map(p => p.slug), [projects]);

    function handleDragEnd(event) {
        if (!canReorder) return;
        const { active, over } = event;
        if (!over || active.id === over.id) return;
        const oldIndex = items.indexOf(active.id);
        const newIndex = items.indexOf(over.id);
        const newList = arrayMove(projects, oldIndex, newIndex);
        setProjects(newList);
        const ordered = newList.map((p, idx) => ({ slug: p.slug, order: idx + 1 }));
        reorderProjects(ordered)
            .then(() => dispatchToast(t('projects.reorderSaved'), { intent: 'success' }))
            .catch(() => dispatchToast(t('projects.reorderFailed'), { intent: 'error' }));
    }

    return (
        <section id={"projects"} className={s.root}>
            <Title3>{t('projects.title')}</Title3>
            {loading ? (
                <div style={{ padding: 32 }}>
                    <Spinner label={t('projects.loading')} />
                </div>
            ) : canReorder ? (
                <DndContext sensors={sensors} collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
                    <SortableContext items={items} strategy={rectSortingStrategy}>
                        <div className={s.grid}>
                            {projects.map(p => (
                                <SortableItem key={p.slug} id={p.slug}>
                                    <ProjectCard project={p} />
                                </SortableItem>
                            ))}
                        </div>
                    </SortableContext>
                </DndContext>
            ) : (
                <div className={s.grid}>
                    {projects.map(p => <ProjectCard key={p.slug} project={p} />)}
                </div>
            )}
            <Toaster />
        </section>
    );
}
