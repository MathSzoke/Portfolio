import { useRef } from 'react';
import { makeStyles } from '@fluentui/react-components';
import { ChatLauncherFloatingButton } from '../components/chat/ChatLauncher.jsx';
import HeroSection from '../components/home/HeroSection.jsx';
import AboutSection from '../components/home/AboutSection.jsx';
import ProjectsSection from '../components/home/ProjectsSection.jsx';
import SocialFloatingBar from '../components/home/SocialFloatingBar.jsx';
import HeaderSection from '../components/home/HeaderSection.jsx';

const useStyles = makeStyles({
    page: {
        display: 'grid',
        justifyContent: 'center',
    },
    content: {
        width: '100%',
        maxWidth: '80%',
        margin: '0 auto',
        padding: '56px 24px',
        display: 'grid',
        gap: '32px',
    },
});

export default function Home() {
    const s = useStyles();
    const projectsRef = useRef<HTMLDivElement | null>null;

    return (
        <>
            <HeaderSection/>
            <div className={s.page}>
                <SocialFloatingBar />
                <div className={s.content}>
                    <HeroSection onSeeProjects={() => projectsRef.current?.scrollIntoView({ behavior: 'smooth' })} />
                    <AboutSection />
                    <ProjectsSection ref={projectsRef} />
                    {/* TO FINISH <ChatLauncherFloatingButton/>*/}
                </div>
            </div>
        </>
    );
}
