import React, { useEffect, useState } from 'react'
import Modal from "@material-ui/core/Modal";
import { DialogContent } from "@material-ui/core";
import { Buffer } from 'buffer';
import Cookies from 'js-cookie';

import './portfolio.css';
import { ProjectsCards } from './projects/ProjectsCards';
import { ProjectExpanded} from './projects/ProjectsModal';

import { notify } from '../notify/notify'

export const Portfolio = () =>
{
    const ref = React.createRef();
    const [projectState, setProject] = useState([]);
    const [open, setOpen] = useState(false);
    const [isClicked, setIsClicked] = useState([]);
    const language = Cookies.get('language');
    const langs = 
    {
        pt: 
        {
            title: "Porfólio",
            titleProjNP: "Números primos | ASP.NET MVC",
            titleProjCourse: "Cursos online | Web API, React.JS",
            lastProjects: "Meus últimos projetos:",
            learnMore: "Clique no projeto para saber mais"
        },
        en:
        {
            title: "Porfolio",
            titleProjNP: "Prime numbers | ASP.NET MVC",
            titleProjCourse: "Online courses | Web API, React.JS",
            lastProjects: "My last projects:",
            learnMore: "Click in the project to learn more"
        }
    }
    const [text, setLanguage] = useState(langs.en.description);
    
    useEffect(() => 
    {
        if (language === 'en') 
        {
            setLanguage(langs.en.description);
        } 
        else if (language === 'pt') 
        {
            setLanguage(langs.pt.description);
        }
    }, [language]);
    

    useEffect(() =>
    {
        const cardInfo = async () => {
            try 
            {
                const contentsCard = [
                    {
                        id: 0,
                        idCard: Buffer.from('Números primos com C#', 'utf8').toString('hex'),
                        idIframe: Date.now() + 0,
                        href: "#primeNumber",
                        title: langs[language].titleProjNP,
                        baseUrl: "https://localhost:44376/"
                    },
                    {
                        id: 1,
                        idCard: Buffer.from("Cursos online", "utf8").toString("hex"),
                        idIframe: Date.now() + 1,
                        href: "#courses",
                        title: langs[language].titleProjCourse,
                        baseUrl: "https://localhost"
                    }
                ];
                setProject(contentsCard);
            }
            catch(err)
            {
                notify("Error: ", err, "error")
                console.log(err);
            }
        }
        cardInfo();
    }, [])

    const handleOpen = (id) => 
    {
        setIsClicked(projectState.find(x => x.id === id));
        setOpen(true);
    };
    
    const handleClose = () => 
    {
        setOpen(false);
        setIsClicked([]);
    };

    return(
        <>
            <section className="portfolio section" id="portfolio">
                <div className="container">
                    <div className="row">
                        <div className="section-title padd-15">
                            <h2>{langs[language].title}</h2>
                        </div>
                    </div>
                    <div className="row">
                        <div className="portfolio-heading padd-15">
                            <h2>{langs[language].lastProjects}</h2>
                            <h3>{langs[language].learnMore}</h3>
                        </div>
                    </div>
                    <div className="row">
                        <div className="portfolio-item padd-15">
                            {
                                projectState.map((project) => (
                                    <ProjectsCards key={project.title} project={project} id={project.id} handleOpen={handleOpen} />
                                ))
                            }
                        </div>
                    </div>
                </div>
            </section>
            <div id="ModalProject">
                <Modal aria-labelledby="transition-modal-title" aria-describedby="transition-modal-description" open={open} onClose={handleClose}>
                    {
                        isClicked && (
                            <DialogContent id='dialogContent' className='dialogContent'>
                                <ProjectExpanded id={`${isClicked.idIframe}-${isClicked.title}`} project={isClicked} ref={ref} />
                            </DialogContent>
                        )
                    }
                </Modal>
            </div>
        </>
    )
}