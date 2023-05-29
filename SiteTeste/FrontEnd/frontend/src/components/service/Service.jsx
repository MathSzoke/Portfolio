import React, { useEffect, useState } from 'react';
import { FaDatabase, FaCode, FaLaptopCode } from 'react-icons/fa';
import './service.css';
import Cookies from 'js-cookie';

export function Service()
{
    const langs = 
    {
        pt: 
        {
            title: "Serviços",
            devWebAPI: "Desenvolvedor Web API",
            descWebAPI: "Desenvolvo WEB API's para diversas ferramentas / sistemas e integro-os para o frontend, com preferência para o React.JS ou Angular.JS",
            devDBA: "Desenvolvedor de Banco de dados",
            descDBA: "Desenvolvo projetos e gerenciamento de dados para Banco de dados, tais como: Procedures, Triggers, Views e queries para relacionar tabelas. Eu trabalho apenas para bancos de dados RELACIONAIS.",
            devSoftware: "Desenvolvedor de Sites",
            descSoftware: "Diferentemente do desenvolvimento de WEB API, eu também posso desenvolver apenas o frontend de um site web, de forma prática, e tento seguir sempre o padrão arquitetural de arquivos."
        },
        en:
        {
            title: "Services",
            devWebAPI: "Web API developer",
            descWebAPI: "I develop WEB API's for several tools / systems and integrate them into the frontend, with preference for React.JS or Angular.JS",
            devDBA: "Data Base developer",
            descDBA: "I develop projects and data management for Database, such as: Procedures, Triggers, Views and queries to relate tables. I work only for RELATIONAL databases.",
            devSoftware: "Software Developer",
            descSoftware: "Differently WEB API development, I can also develop just the frontend of a website, in a practical way, and I always try to follow the architectural pattern of files."
        }
    }
  
    const language = Cookies.get('language');
    
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
    
    const [text, setLanguage] = useState(langs.en.description);

    return (
        <section className="service section" id="services">
            <div className="container">
                <div className="row">
                    <div className="section-title padd-15">
                        <h2>{langs[language].title}</h2>
                    </div>
                </div>
                <div className="row">
                    <div className="service-item padd-15">
                        <div className="service-item-inner">
                            <div className="icon">
                                <i className="fa fa-laptop-code"><FaLaptopCode/></i>
                            </div>
                            <h4 style={{cursor: "default",userSelect:"none"}}>{langs[language].devWebAPI}</h4>
                            <p style={{cursor: "default",userSelect:"none"}}>{langs[language].descWebAPI}</p>
                        </div>
                    </div>
                    <div className="service-item padd-15">
                        <div className="service-item-inner">
                            <div className="icon">
                                <i className="fa fa-code"><FaCode/></i>
                            </div>
                            <h4 style={{cursor: "default",userSelect:"none"}}>{langs[language].devSoftware}</h4>
                            <p style={{cursor: "default",userSelect:"none"}}>{langs[language].descSoftware}</p>
                        </div>
                    </div>
                    <div className="service-item padd-15">
                        <div className="service-item-inner">
                            <div className="icon">
                                <i className="fa fa-database"><FaDatabase/></i>
                            </div>
                            <h4 style={{cursor: "default",userSelect:"none"}}>{langs[language].devDBA}</h4>
                            <p style={{cursor: "default",userSelect:"none"}}>{langs[language].descDBA}</p>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
}
