import React from 'react';
import './about.css';
import Cookies from 'js-cookie';

export function About()
{
    const langs = 
    {
        pt: 
        {
            description:
            "Comecei a estudar programação muito antes de ter iniciado em uma graduação. A partir dos estudos básicos, " +
            "comecei a me identificar mais com a área, comecei pelo básico como HTML, CSS, JavaScript, e parti para backend utilizando PHP e MySQL. " +
            "A partir disso decidi começar a estudar tecnologias mais dificeis, pois é assim que eu vejo que consigo aprender mais. Encarando dificuldades! " +
            "Testei muitas formas de códigos e aprendi alguns padrões de design (design patterns) para empresas. Tento me adaptar todos os dias inserindo meus aprendizados " +
            "nas aplicações e projetos, mesmo que sejam pequenos. " +
            "Atualmente me identifico total como Desenvolvedor Fullstack, pois ao meu ver, é muito mais prático ter o conhecimento do que você faz tanto no servidor" +
            "quanto no lado do cliente. Cada possivel erro que ocorra, eu tento sempre apresentar para o cliente de forma tratada e que não atrase a vida do mesmo.",
            title: "Sobre mim",
            im: "Meu nome é Matheus Henrique Szoke La Motta e ",
            spanDev: "Desenvolvedor de web site",
            born: "Nasci em: ",
            age: "Idade: ",
            graduation: "Graduação: ",
            cell: "Celular: ",
            city: "Cidade: ",
            available: "Disponível",
            contact: "Contate-me",
            education: "Educação",
            experience: "Experiência",
            dateEducation1: "out 2021 - dez 2023",
            gradADS: "Graduação Análise e Desenvolvimento de Sistemas",
            gradCC: "Graduação Ciência da Computação",
            course: "Cursos online",
            descGradADS1: "- Atualmente no segundo semestre. ",
            descGradADS2: "Iniciei a minha graduação em ADS com intenção de me aprofundar mais na área de tecnologia, após ter iniciado um curso técnico em programação que falhou infelizmente pois a instituição de ensino faliu...",
            descGradCC1: "- Atualmente no segundo semestre. ",
            descGradCC2: "Alguns meses depois de ter iniciado a graduação em ADS e ter analisado mais o que ensinam e a grade, percebi que é a minha área realmente, mas também, notei que ADS é apenas um curso técnico, por isso, iniciei em mais uma graduação e na qual me vejo aprendendo mais profundamente a arte da programação.",
            courseTecArbyte: "- Curso tecnico pelo Arbyte (a empresa faliu e eu não conclui o curso)",
            coursesAlura: "- Cursos especializantes pelo Alura",
            coursesCompleted: "- C#, Lógica de programação, Modelagem de Banco de Dados, SQL Server, PHP, JavaScript, HTML e CSS, Oracle Database, MySQL Server, Web API, SOLID.",
            coursesDescription: "Cursos feitos com intenções nítidas de me aprimorar na programação, e principalmente a lidar com consequencias, aprender a lidar com erros de sistemas, e a lógica toda por trás, cursos feitos pelo Alura são perfeitos, além de existir vários tipos de cursos de várias linguagens, as video-aulas são bem didáticas e explicativas.",
            internship1: "Estágio em Programação Backend - AJV Sistemas LTDA",
            internship2: "Estágio em Programação FullStack - Ka Solution",
            dateInternship1: "abril 2021 - out 2021",
            dateInternship2: "jan 2022 - ...",
            descInternship1: "No meio do ano de 2021, em meu primeiro semestre da faculdade (Estácio) em que eu cursava, consegui o meu primeiro trabalho, sendo o mesmo um estágio exatamente da minha área, onde me identifiquei muito. Apesar de ter aprendido o suficiente sobre como funciona uma empresa e como funciona o backend de uma empresa, eu precisei sair por questões pessoais e porquê era um lugar razoavelmente longe e que não pagava o necessário para a faculdade em que eu frequentava.",
            descInternship1Again: "Além da vaga dizer que era necessário conhecimento em C#, eu sempre utilizei Java lá, o que me desanimou um pouco.",
            descInternship2: "Este é o meu segundo emprego, e segundo estágio na mesma área em que eu consegui novamente no primeiro semestre de outra faculdade em que agora estou cursando e não irei sair.",
            descInternship2Again: "Além da vaga dizer que era necessário conhecimento em C#, eu sempre utilizei Java lá, o que me desanimou um pouco.",
            descInternship2Again2: "Vaga muito boa, 100% remota, Fullstack e estou adorando participar dessa oportunidade. Porém, tem uns defeitos que eu considero bem grave, " +
            "como por exemplo: meu primeiro PC recebido pela empresa houve defeitos quando me mudei para uma cidade na qual a energia é apenas 220V, o que, " +
            "eu acredito que tenha causado algum defeito na máquina. Sempre que eu desligava a máquina, eu precisava ter a sorte dele ligar de novo ou fazer " +
            "alguma bruxaria para que ele ligasse de novo. E mesmo contatando o suporte sobre o problema, o que eles me recomendaram foi: deixar o pc ligado sempre (???), " +
            "isso resultou em: contas de energia cara e provavelmente mais defeitos para a máquina, o que inclusive não recebi nenhum auxilio para isso e que demoraram alguns meses para que " +
            "quisessem finalmente me enviar uma nova máquina, depois de reclamar novamente sobre o problema. Com isso, vem o segundo defeito: eu tive que literalmente levar a máquina por 3 horas " +
            "no trem, ônibus e metrô até a empresa, para que eu pegasse a máquina nova para mim trabalhar (???).",
            descInternship2Again3: "O terceiro defeito eu ainda acho mais problemático do que os outros dois, pois os DEVs não possuem o gestor exclusivo da área, o que significa: falta de consenso para o desempenho dos profissionais, " +
            "falta de comunicação entre (esse problema não é nem por não possuir o gestor exclusivo da área, mas sim, o problema está no gestor, pois ele realmente não se comunica com os Devs, nós tinhamos que entrar " +
            "em contato com ele sempre para saber de novidades / questões de feriados por exemplo, o que tornou chato), outro problema é Contrato por Desempenho. O contrato não especificava que a vaga havia aumento de " +
            "remuneração por desempenho do funcionário, mas, o mínimo de esforço para o reconhecimento de evolução do funcionário e recompensá-lo por isso, seria extremamente legal e satisfatório para o funcionário. " +
            "Eu senti uma grande evolução na minha carreira, na minha mentalidade e nas soluções de problemas, e apesar de ter passado por problemas dos clientes sozinho pois meu parceiro ou havia entrado de férias" +
            "ou não pôde comparecer, eu consegui solucioná-los e de forma mais rápido que eu pude. Ter passado 1 ano inteiro com o mesmo formato de trabalho e no próximo ano continuar a mesma coisa, chega a ser decepcionante.",
            descInternship2Again4: "Com isso posso dizer que o único lado positivo dessa vaga ao meu ver é: 100% home office.",
            technologiesUsed: "Tecnologias utilizadas: ",
            tecUsed1: "Um pouco de ASP.NET MVC",
            tecUsed5: "Um pouco de React Native com Expo",
            now: "Agora"
        },
        en: 
        {
            description:
            "I started studying programming long before I started an undergraduate course. From basic studies, " +
            "I started to identify myself more with the area, I started with the basics like HTML, CSS, JavaScript, and started with the backend using PHP and MySQL. " +
            "From that, I decided to start studying more difficult technologies, because that's how I see that I can learn more. Facing difficulties! " +
            "I tested many forms of code and learned some design patterns for companies. I try to adapt every day by inserting my learnings " +
            "in applications and projects, even if they are small. " +
            "I currently identify myself as a Fullstack Developer, because in my view, it is much more practical to have knowledge of what you do so much on the server" +
            "as well as on the client side. Every possible error that occurs, I always try to present it to the client in a treated way and that does not delay his life.",
            title: "About me",
            im: "My name is Matheus Henrique Szoke La Motta and ",
            spanDev: "Developer web site",
            born: "I was born in: ",
            age: "Age: ",
            graduation: "University graduate: ",
            cell: "Cellphone: ",
            city: "City: ",
            available: "Available",
            contact: "Contact me",
            education: "Education",
            experience: "Experience",
            dateEducation1: "oct 2021 - dec 2023",
            gradADS: "Bachelor's Degree Systems Analysis and Development",
            gradCC: "Bachelor's Degree Science Computer",
            course: "Online course",
            descGradADS1: "- Currently in the second semester. ",
            descGradADS2: "I started my graduation in ADS with the intention of going deeper into the technology area, after starting a technical course in programming that unfortunately failed because the educational institution went bankrupt...",
            descGradCC1: "- Currently in the second semester. ",
            descGradCC2: "A few months after starting the ADS graduation and having analyzed more what they teach and the grade, I realized that it is really my area, but also, I noticed that ADS is just a technical course, so I started in another graduation and in which I see myself learning more deeply the art of programming.",
            courseTecArbyte: "- Technical course through Arbyte (the company went bankrupt and I did not complete the course)",
            coursesAlura: "- Specialized courses by Alura",
            coursesCompleted: "- C#, Logic Programming, Database Modeling, SQL Server, PHP, JavaScript, HTML and CSS, Oracle Database, MySQL Server, Web API, SOLID.",
            coursesDescription: "Courses made with clear intentions to improve myself in programming, and mainly to deal with consequences, learn to deal with system errors, and the whole logic behind it, courses made by Alura are perfect, besides there are several types of courses in several languages , the video lessons are very didactic and explanatory.",
            internship1: "Internship in Backend Programming - AJV Sistemas LTDA",
            internship2: "Internship in FullStack Programming - Ka Solution",
            dateInternship1: "april 2021 - oct 2021",
            dateInternship2: "jan 2022 - ...",
            descInternship1: "In the middle of 2021, in my first semester of college (Estácio) where I was studying, I got my first job, which was an internship in exactly my area, where I identified myself a lot. Despite having learned enough about how a company works and how a company's backend works, I had to leave for personal reasons and because it was a reasonably far away place and that didn't pay the necessary for the college I attended.",
            descInternship1Again: "In addition to the vacancy saying that knowledge in C# was necessary, I always used Java there, which discouraged me a little.",
            descInternship2: "This is my second job, and second internship in the same area where I got it again in the first semester of another college that I am now attending and I will not be leaving.",
            descInternship2Again: "In addition to the vacancy saying that knowledge in C# was necessary, I always used Java there, which discouraged me a little.",
            descInternship2Again2: "Very good vacancy, 100% remote, Fullstack and I'm loving participating in this opportunity. However, it has some flaws that I consider very serious, " +
            "for example: my first PC received by the company there were defects when I moved to a city in which the energy is only 220V, which, " +
            "I believe it caused some malfunction in the machine. Whenever I turned the machine off, I had to be lucky for it to turn back on or do " +
            "some witchcraft to make it turn on again. And even contacting support about the problem, what they recommended was: leave the pc on always (???), " +
            "this resulted in: expensive energy bills and probably more malfunctions for the machine, which I didn't even receive any help for and that took a few months for " +
            "they finally wanted to send me a new machine, after complaining again about the problem. With that comes the second defect: I literally had to take the machine for 3 hours " +
            "on the train, bus and subway to the company, so that I could take the new machine to work with me (???).",
            descInternship2Again3: "The third defect I still find more problematic than the other two, because the DEVs do not have the exclusive manager of the area, which means: lack of consensus for the performance of the professionals, " +
            "lack of communication between (this problem is not even because we do not have the exclusive manager of the area, but rather, the problem is in the manager, because he really does not communicate with the Devs, we had to enter " +
            "always in touch with him for news / holiday issues for example, which made it annoying), another problem is Contract for Performance. The contract did not specify that the vacancy was increased by " +
            "remuneration for the employee's performance, but the minimum of effort for the recognition of the employee's evolution and rewarding him for it, would be extremely cool and satisfying for the employee. " +
            "I felt a great evolution in my career, in my mindset and in my problem solving, and despite having gone through customer problems alone because my partner had either gone on vacation " +
            "or could not attend, I managed to solve them and as quickly as I could. Having spent 1 whole year with the same work format and the next year continuing the same thing is disappointing.",
            descInternship2Again4: "With that, I can say that the only positive side of this vacancy in my view is: 100% home office.",
            technologiesUsed: "Technologies used:",
            tecUsed1: "A bit of ASP.NET MVC",
            tecUsed5: "A bit of React Native with Expo",
            now: "Now"
        }
    }

    const language = Cookies.get('language');

    return (
        <section className="about section" id="about">
            <div className="container">
                <div className="row">
                    <div className="section-title padd-15">
                        <h2>{langs[language].title}</h2>
                    </div>
                </div>
                <div className="row">
                    <div className="about-content padd-15">
                        <div className="row">
                            <div className="about-text padd-15">
                                <h3>{langs[language].im}<span>{langs[language].spanDev}</span></h3>
                                <p>{langs[language].description}</p>
                            </div>
                        </div>
                        <div className="row">
                            <div className="personal-info padd-15">
                                <div className="row">
                                    <div className="info-item padd-15">
                                        <p>{langs[language].born}<span>08 dec 2001</span></p>
                                    </div>
                                    <div className="info-item padd-15">
                                        <p>{langs[language].age}<span>21</span></p>
                                    </div>
                                    <div className="info-item padd-15">
                                        <p>Email: <span>Matheusszoke@gmail.com</span></p>
                                    </div>
                                    <div className="info-item padd-15">
                                        <p>{langs[language].graduation}<span>ADS & CC</span></p>
                                    </div>
                                    <div className="info-item padd-15">
                                        <p>{langs[language].cell}<span>+55 (11) 991388-1138</span></p>
                                    </div>
                                    <div className="info-item padd-15">
                                        <p>{langs[language].city}<span>São Paulo</span></p>
                                    </div>
                                    <div className="info-item padd-15">
                                        <p>Freelance: <span>{langs[language].available}</span></p>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="buttons padd-15">
                                        <a href="#contact" data-section-index="1" className="btn hire-me">{langs[language].contact}</a>
                                    </div>
                                </div>
                            </div>
                            <div className="skills padd-15">
                                <div className="row">
                                    <div className="skill-item padd-15">
                                        <h5>HTML</h5>
                                        <div className="progress">
                                            <div className="progress-in" style={{width:"84%"}}></div>
                                            <div className="skill-percent">84%</div>
                                        </div>
                                    </div>
                                    <div className="skill-item padd-15">
                                        <h5>CSS</h5>
                                        <div className="progress">
                                            <div className="progress-in" style={{width:"84%"}}></div>
                                            <div className="skill-percent">84%</div>
                                        </div>
                                    </div>
                                    <div className="skill-item padd-15">
                                        <h5>JavaScript - JQuery - AJAX - React.JS</h5>
                                        <div className="progress">
                                            <div className="progress-in" style={{width:"76%"}}></div>
                                            <div className="skill-percent">76%</div>
                                        </div>
                                    </div>
                                    <div className="skill-item padd-15">
                                        <h5>C# - .NET - ASP.NET</h5>
                                        <div className="progress">
                                            <div className="progress-in" style={{width:"61%"}}></div>
                                            <div className="skill-percent">61%</div>
                                        </div>
                                    </div>
                                    <div className="skill-item padd-15">
                                        <h5>SQL Server</h5>
                                        <div className="progress">
                                            <div className="progress-in" style={{width:"55%"}}></div>
                                            <div className="skill-percent">55%</div>
                                        </div>
                                    </div>
                                    <div className="skill-item padd-15">
                                        <h5>PostgreSQL</h5>
                                        <div className="progress">
                                            <div className="progress-in" style={{width:"45%"}}></div>
                                            <div className="skill-percent">45%</div>
                                        </div>
                                    </div>
                                    <div className="skill-item padd-15">
                                        <h5>MySQL</h5>
                                        <div className="progress">
                                            <div className="progress-in" style={{width:"65%"}}></div>
                                            <div className="skill-percent">65%</div>
                                        </div>
                                    </div>
                                    <div className="skill-item padd-15">
                                        <h5>Git</h5>
                                        <div className="progress">
                                            <div className="progress-in" style={{width:"45%"}}></div>
                                            <div className="skill-percent">45%</div>
                                        </div>
                                    </div>
                                    <div className="skill-item padd-15">
                                        <h5>TFS</h5>
                                        <div className="progress">
                                            <div className="progress-in" style={{ width: "76%" }}></div>
                                            <div className="skill-percent">76%</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="education padd-15">
                                <h3 className="title">{langs[language].education}</h3>
                                <div className="row">
                                    <div className="timeline-box padd-15">
                                        <div className="timeline shadow-dark">
                                            <div className="timeline-item">
                                                <div className="circle-dot">
                                                    <h3 className="timeline-date">
                                                        <i className="fa fa-calendar"></i> {langs[language].dateEducation1}
                                                    </h3>
                                                    <h4 className="timeline-title">{langs[language].gradADS}</h4>
                                                    <p className="timeline-text">{langs[language].descGradADS1}<br></br>{langs[language].descGradADS2}</p>
                                                </div>
                                            </div>
                                            <div className="timeline-item">
                                                <div className="circle-dot">
                                                    <h3 className="timeline-date">
                                                        <i className="fa fa-calendar"></i> jan 2022 - jan 2026
                                                    </h3>
                                                    <h4 className="timeline-title">{langs[language].gradCC}</h4>
                                                    <p className="timeline-text">{langs[language].descGradCC1}<br></br>{langs[language].descGradCC2}</p>
                                                </div>
                                            </div>
                                            <div className="timeline-item">
                                                <div className="circle-dot">
                                                    <h3 className="timeline-date">
                                                        <i className="fa fa-calendar"></i> 2020 - ...
                                                    </h3>
                                                    <h4 className="timeline-title">{langs[language].course}</h4>
                                                    <p className="timeline-text">
                                                        {langs[language].courseTecArbyte}<br />
                                                        {langs[language].coursesAlura}<br />
                                                        {langs[language].coursesCompleted}<br />
                                                        {langs[language].coursesDescription}
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="experience padd-15">
                                <h3 className="title">{langs[language].experience}</h3>
                                <div className="row">
                                    <div className="timeline-box padd-15">
                                        <div className="timeline shadow-dark">
                                            <div className="timeline-item">
                                                <div className="circle-dot">
                                                    <h3 className="timeline-date">
                                                        <i className="fa fa-calendar"></i> {langs[language].dateInternship1}
                                                    </h3>
                                                    <h4 className="timeline-title">{langs[language].internship1}</h4>
                                                    <p className="timeline-text">
                                                        {langs[language].descInternship1}<br/>
                                                        {langs[language].descInternship1Again}
                                                        <br></br>
                                                        <br></br>
                                                        {langs[language].technologiesUsed}
                                                        <li className='tecnoligies'>{langs[language].tecUsed1}</li>
                                                        <li className='tecnoligies'>Java</li>
                                                        <li className='tecnoligies'>SQL Oracle</li>
                                                        <li className='tecnoligies'>Postman</li>
                                                        <li className='tecnoligies'>Source Tree</li>
                                                        <li className='tecnoligies'>{langs[language].tecUsed5}</li>
                                                    </p>
                                                </div>
                                            </div>
                                            <div className="timeline-item">
                                                <div className="circle-dot">
                                                    <h3 className="timeline-date">
                                                        <i className="fa fa-calendar"></i> {langs[language].dateInternship2} {langs[language].now}
                                                    </h3>
                                                    <h4 className="timeline-title">{langs[language].internship2}</h4>
                                                    <p className="timeline-text">{langs[language].descInternship2} <br />
                                                        {langs[language].descInternship2Again} <br/>
                                                        {langs[language].descInternship2Again2} <br/>
                                                        {langs[language].descInternship2Again3} <br/>
                                                        {langs[language].descInternship2Again4}

                                                        <br></br>
                                                        <br></br>
                                                        {langs[language].technologiesUsed}
                                                        <li className='tecnoligies'>ASP.NET MVC</li>
                                                        <li className='tecnoligies'>ASP.NET Core 4</li>
                                                        <li className='tecnoligies'>SQL Server</li>
                                                        <li className='tecnoligies'>TFS</li>
                                                        <li className='tecnoligies'>HelpDesk</li>
                                                        <li className='tecnoligies'>JavaScript - JQuery</li>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
}