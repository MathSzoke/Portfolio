import React, { Component }  from 'react';
import { FaPhone, FaLinkedin, FaEnvelope, FaWhatsapp } from 'react-icons/fa';
import './contact.css';
import { notify } from '../notify/notify'
import { ClipLoader } from 'react-spinners';
import Cookies from 'js-cookie';

export class Contact extends Component
{
    static displayName = Contact.name;

    state = {
        language: 'en',
        loading: false,
        langs: 
        {
            pt:
            {
                title: "Contate-me",
                sendMessage: "Enviar mensagem",
                placeholderName: "Nome",
                placeholderSubject: "Assunto",
                placeholderMessage: "Mensagem",
                callMe: "Ligue-me",
                anyQuestion: "Você possui alguma dúvida?",
                imAtYourService: "Estou ao seu dispôr",
                sendMeAnEmail: "Me envie um email",
                imReceptive: "Sou muito receptivo às mensagens",
                errorCharacter: "Nome não pode conter caracteres especiais!",
                sendSuccessfully: "Email enviado com sucesso!",
                errorSubmited: "Ocorreu um erro inesperado enquanto envia o email.",
                unexpectedError: "Ocorreu um erro inesperado: ",
                sendHeader: "Enviado!",
                errorHeader: "Erro",
                errorNameEmpty: "O campo 'Nome' não pode estar vázio!",
                errorEmailEmpty: "O campo 'Email' não pode estar vázio!",
                errorSubjectEmpty: "O campo 'Assunto' não pode estar vázio!",
                errorMessageEmpty: "O campo 'Mensagem' não pode estar vázio!",
                errorEmpty: "Não pode enviar uma mensagem vázia, também não ia ser legal receber um email sem nada...",
                errorEmailFormat: "O formato de email está incorreto, não esqueça de adicionar o '@dominio.com'"
            },
            en: 
            {
                title: "Contact me",
                sendMessage: "Send Message",
                placeholderName: "Name",
                placeholderSubject: "Subject",
                placeholderMessage: "Message",
                callMe: "Call me",
                anyQuestion: "Do you have any questions?",
                imAtYourService: "I'm at your service",
                sendMeAnEmail: "Send me an email",
                imReceptive: "I am very receptive to messages",
                errorCharacter: "Name cannot contain special character!",
                sendSuccessfully: "Email successfully sent!",
                errorSubmited: "An error occurred while submitting the form",
                unexpectedError: "An error unexpected ocurred: ",
                sendHeader: "Sent!",
                errorHeader: "Error",
                errorNameEmpty: "Field 'Name' cannot be empty!",
                errorEmailEmpty: "Field 'Email' cannot be empty!",
                errorSubjectEmpty: "Field 'Subject' cannot be empty!",
                errorMessageEmpty: "Field 'Message' cannot be empty!",
                errorEmpty: "You can't send an empty message, it wouldn't be nice to receive an email with nothing either...",
                errorEmailFormat: "The email format is incorrect, don't forget to add '@domain.com'"
            },
        },
    }
    
    componentDidMount() {
        const language = Cookies.get('language');
        this.setState({ language });
    }

    handleSubmit = async (event) =>
    {
        event.preventDefault();

        this.setState({ loading: true });

        const language = Cookies.get('language');

        function isValidName(name)
        {
            // Allow only letters and spaces
            const regex = /^[a-zA-Z\s]+$/;
            return regex.test(name);
        }

        const name = this.nameInput.value;
        const email = this.emailInput.value;
        const subject = this.subjectInput.value;
        const message = this.messageInput.value;

        if(name === "" && email === "" && subject === "" && message === "")
        {
            this.setState({ loading: false });
            throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorEmpty, "error");
        }
        else if(name === "")
        {
            this.setState({ loading: false });
            throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorNameEmpty, "error");
        }
        else if(email === "")
        {
            this.setState({ loading: false });
            throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorEmailEmpty, "error");
        }
        else if(subject === "")
        {
            this.setState({ loading: false });
            throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorSubjectEmpty, "error");
        }
        else if(message === "")
        {
            this.setState({ loading: false });
            throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorMessageEmpty, "error");
        }

        if (!isValidName(name))
        {
            this.setState({ loading: false });
            throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorCharacter, "error");
        }
        
        // Get form data
        const formData = {
            Name: name,
            Email: email,
            Subject: subject,
            Message: message
        };

        const headers = {'Content-Type': 'application/json', 'Access-Control-Allow-Origin': '*'}

        // Send form data to ContactController
        try
        {
            const response = await fetch('https://localhost:44395/Contact', {
                method: 'POST',
                statusCode: 201,
                body: JSON.stringify(formData),
                headers: headers
            });

            if (response.ok)
            {
                // Form success send
                notify(this.state.langs[language].sendHeader, this.state.langs[language].sendSuccessfully, "success");
            }
            else if(response.status === 400)
            {
                notify(this.state.langs[language].errorHeader, this.state.langs[language].errorEmailFormat, "error");
            }
            else 
            {
                // Some occurred error type
                throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorSubmited, "error");
            }
        }
        catch (error)
        {
            notify(this.state.langs[language].errorHeader, this.state.langs[language].unexpectedError + error, "error");
        }

        this.setState({ loading: false });
    }

    render()
    {
        const { language, langs } = this.state;

        return (
            <section className="contact section" id="contact">
                <div className="container">
                    <div className="row">
                        <div className="section-title padd-15">
                            <h2>{langs[language].title}</h2>
                        </div>
                    </div>
                    <h3 className="contact-title padd-15">{langs[language].anyQuestion}</h3>
                    <h4 className="contact-sub-title padd-15">{langs[language].imAtYourService}</h4>
                    <div className="row">
                        <div className="contact-info-item padd-15">
                            <div className="icon"><FaPhone className="fa" title="Telefone"/></div>
                            <h4>{langs[language].callMe}</h4>
                            <a style={{fontSize: "16px"}} className="contact-title" href="" target="_blank">+55 (11) 99138-1138</a>
                        </div>
                        <div className="contact-info-item padd-15">
                            <div className="icon"><FaLinkedin className="fa" title="Linkedin"/></div>
                            <h4>LinkedIn</h4>
                            <a style={{fontSize: "16px"}} className="contact-title" href="https://www.linkedin.com/in/matheus-henrique-szoke-la-motta-b819241a5/" target={"_blank"}>Matheus Szoke</a>
                        </div>
                        <div className="contact-info-item padd-15">
                            <div className="icon"><FaEnvelope className="fa" title="Email"/></div>
                            <h4>Email</h4>
                            <a href="#name" style={{fontSize: "16px"}} className="contact-title">matheusszoke@gmail.com</a>
                        </div>
                        <div className="contact-info-item">
                            <div className="icon"><FaWhatsapp className="fa" title="Whatsapp"/></div>
                            <h4>WhatsApp</h4>
                            <a style={{fontSize: "16px"}} className="contact-title" href="https://api.whatsapp.com/send?phone=5511991381138&" target={"_blank"}>+55 (11) 99138-1138</a>
                        </div>
                    </div>
                    <h3 className="contact-title padd-15">{langs[language].sendMeAnEmail}</h3>
                    <h4 className="contact-sub-title padd-15">{langs[language].imReceptive}</h4>
                    <form id="form" onSubmit={this.handleSubmit}>
                        <div className="row">
                            <div className="contact-form padd-15">
                                <div className="row">
                                    <div className="form-item col-6 padd-15">
                                        <div className="form-group">
                                            <input className="form-control" type="text" name="name" id="name" required="" placeholder={langs[language].placeholderName} maxLength="70" ref={input => this.nameInput = input} />
                                        </div>
                                    </div>
                                    <div className="form-item col-6 padd-15">
                                        <div className="form-group">
                                            <input className="form-control" type="email" name="email" id="email" placeholder="Email" required="" maxLength="80" ref={input => this.emailInput = input} />
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="form-item col-12 padd-15">
                                        <div className="form-group">
                                            <input className="form-control" type="subject" name="subject" id="subject" placeholder={langs[language].placeholderSubject} required="" maxLength="255" ref={input => this.subjectInput = input} />
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="form-item col-12 padd-15">
                                        <div className="form-group">
                                            <textarea className="form-control" id="message" name="message" rows="5" required="" placeholder={langs[language].placeholderMessage} maxLength="8000" ref={input => this.messageInput = input} />
                                            <p id="written"></p>
                                            <p id="remaining"></p>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="form-item col-12 padd-15">
                                        <button id="submit" type="submit" className="btn">{ this.state.loading ? <ClipLoader /> : this.state.langs[language].sendMessage }</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </section>
        );
    }
}
