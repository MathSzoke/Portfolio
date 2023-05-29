import React, { useEffect, useState, useRef } from 'react';
import ME from './images/me.jpg';
import Curriculum from './files/MatheusSzoke_Curriculo.pdf';
import './home.css';
import Cookies from 'js-cookie';
import Typed from "typed.js";

export function Home ()
{
    const langs = 
    {
        pt: 
        {
            myNameIs: "Olá, meu nome é ",
            im: "Eu sou um ",
            dev: "Desenvolvedor de Software",
            carrier: "Sou desenvolvedor de software com vasta experiência há mais de 2 anos. " +
            "Minha experiência é desenvolvimento FullStack, ideias lógicas para o funcionamento interno de um software, " +
            "desenvolvimento de Banco de Dados e uma experiência leve com front-end, " +
            "que tento aplicar no meu dia a dia com o back-end, e muito mais...",
            seeCV: "Ver curriculo",
            devWebAPI: "Desenvolvedor Web API",
            devDBA: "Desenvolvedor de Banco de dados",
            devSoftware: "Desenvolvedor de Sistemas"
        },
        en:
        {
            myNameIs: "Hello, my name is ",
            im: "I'm an ",
            dev: "Software Developer",
            carrier: "I am a software developer with extensive experience for over 2 years. " +
            "My experience is FullStack development, logical ideas for the inner workings of a software, " +
            "Database development and a lightweight front-end experience, " +
            "that I try to apply in my day to day with the backend, and much more...",
            seeCV: "See resume",
            devWebAPI: "Web API developer",
            devDBA: "Database developer",
            devSoftware: "Software Developer"
        }
    }
    const el = useRef(null);

    const language = Cookies.get('language');

    useEffect(() => 
    {
        const typed = new Typed(el.current,
        {
            strings: ["", langs[language].devWebAPI, langs[language].devDBA, langs[language].devSoftware],
            typeSpeed: 100,
            BackSpeed: 60,
            loop: true
        })
        return () => {
          typed.destroy();
        };
    }, []);

    return (
        <section className="home active section" id="home">
            <div className="container">
                <div className="row">
                    <div className="home-info padd-15">
                        <h3 className="hello">{langs[language].myNameIs} <span className="name">Matheus Szoke</span></h3>
                        <h3 className="my-profession">{langs[language].im}<span ref={el} className="typing"></span></h3>
                        <p>{langs[language].carrier}</p>
                        <a href={ Curriculum } className="btn" target={'_blank'} rel="noreferrer">{langs[language].seeCV}</a>
                    </div>
                    <div className="home-img padd-15">
                        <img src={ ME } alt="" />
                    </div>
                </div>
            </div>
        </section>
    );
}
