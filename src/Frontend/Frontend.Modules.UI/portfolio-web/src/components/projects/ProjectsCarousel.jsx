import React from 'react';
import {
    Carousel,
    CarouselNav,
    CarouselNavButton,
    CarouselNavContainer,
    CarouselSlider,
    CarouselViewport,
    makeStyles,
} from '@fluentui/react-components';

const useClasses = makeStyles({
    container: {
        display: 'grid',
        gridTemplateColumns: 'minmax(0, 1fr)',
        gap: '12px',
    },
    carousel: {
        padding: '12px 0',
    },
});

const getAnnouncement = (index, totalSlides) =>
    `Carrossel: slide ${index + 1} de ${totalSlides}`;

export const ProjectsCarousel = ({ children }) => {
    const classes = useClasses();

    return (
        <div className={classes.container}>
            <Carousel
                className={classes.carousel}
                announcement={getAnnouncement}
                align="center"
                whitespace={false}
            >
                <CarouselViewport>
                    <CarouselSlider
                        cardFocus
                        aria-label="Use as setas para navegar entre os projetos"
                    >
                        {children}
                    </CarouselSlider>
                </CarouselViewport>
                <CarouselNavContainer
                    layout="inline"
                    next={{ 'aria-label': 'Próximo' }}
                    prev={{ 'aria-label': 'Anterior' }}
                >
                    <CarouselNav>
                        {(index) => (
                            <CarouselNavButton
                                aria-label={`Ir para o slide ${index + 1}`}
                            />
                        )}
                    </CarouselNav>
                </CarouselNavContainer>
            </Carousel>
        </div>
    );
};