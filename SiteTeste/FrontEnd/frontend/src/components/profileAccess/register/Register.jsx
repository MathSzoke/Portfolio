import React, { Component } from "react";
import './register.css';
import Cookies from 'js-cookie';
import { ClipLoader } from 'react-spinners';
import { notify } from '../../notify/notify'

const language = Cookies.get('language');
export class Register extends Component
{
    state = {
        language: 'en',
        loading: false,
        langs: 
        {
            pt: 
            {
                emailInput: "Email",
                passwordInput: "Senha",
                confirmPasswordInput: "Confirme a senha",
                registerTitle: "Registrar",
                errorSubmited: "Ocorreu um erro inesperado enquanto envia o email.",
                unexpectedError: "Ocorreu um erro inesperado: ",
                sendHeader: "Enviado!",
                errorHeader: "Erro",
                errorEmailEmpty: "O campo 'Email' não pode estar vázio!",
                errorPasswordEmpty: "O campo 'Senha' não pode estar vázio!",
                errorEmpty: "Não pode enviar uma mensagem vázia, também não ia ser legal receber um email sem nada...",
                errorEmailFormat: "O formato de email está incorreto, não esqueça de adicionar o '@dominio.com'",
                errorUnauthorized: "Parece que o email não existe, tente criar uma conta com este email.",
                errorEmailNotFound: "Parece que não consegui encontrar o seu email ou senha no sistema",
                changePass: "Esqueci a senha",
                sendSuccessfully: ""
            },
            en:
            {
                emailInput: "Email",
                passwordInput: "Password",
                confirmPasswordInput: "Confirm the password",
                registerTitle: "Register",
                errorSubmited: "An error occurred while submitting the form",
                unexpectedError: "An error unexpected ocurred: ",
                sendHeader: "Sent!",
                errorHeader: "Error",
                errorEmailEmpty: "Field 'Email' cannot be empty!",
                errorPasswordEmpty: "Field 'Subject' cannot be empty!",
                errorEmpty: "You can't send an empty message, it wouldn't be nice to receive an email with nothing either...",
                errorEmailFormat: "The email format is incorrect, don't forget to add '@domain.com'",
                errorUnauthorized: "It seems that the email does not exist, try to create an account with this email.",
                errorEmailNotFound: "It looks like I couldn't find your email or password in the system",
                changePass: "Forgot password",
                sendSuccessfully: ""
            },
        },
    }

    getCacheToken = async () => 
    {
        try 
        {
            const email = this.emailInput.value;
            const response = await fetch("https://localhost:44395/UserLogin/" + email);
            const data = await response.text();
            Cookies.set('tokenAcess', data);

            if(response.status === 409)
            {
                this.setState({ loading: false });
                throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorEmailNotFound, "error");
            }

            console.log(data);
        }
        catch (error) 
        {
            console.error(error);
        }
    };

    componentDidMount() {
        const language = Cookies.get('language');
        this.setState({ language });
    }
    
    handleSubmit = async (event) =>
    {
        const button = document.getElementById('submitLogin')
        event.preventDefault();

        this.setState({ loading: true });
        button.setAttribute("disabled", "disabled");

        const email = this.emailInput.value;
        const pass = this.passwordInput.value;

        if(email === "" && pass === "")
        {
            this.setState({ loading: false });
            button.removeAttribute("disabled");
            throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorEmpty, "error");
        }
        else if(email === "")
        {
            this.setState({ loading: false });
            button.removeAttribute("disabled");
            throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorEmailEmpty, "error");
        }
        else if(pass === "")
        {
            this.setState({ loading: false });
            button.removeAttribute("disabled");
            throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorPasswordEmpty, "error");
        }

        // Get form data
        const formData = {
            Email: email,
            Password: pass
        };

        const headers = {'Content-Type' : 'application/json', 'Access-Control-Allow-Origin' : '*'}

        // Send form data to UserLoginController
        try
        {
            const response = await fetch('https://localhost:44395/UserLogin', {
                method: 'POST',
                statusCode: 201,
                body: JSON.stringify(formData),
                headers: headers
            });

            switch(response.status)
            {
                case 200:
                    notify(this.state.langs[language].sendHeader, this.state.langs[language].sendSuccessfully, "success");
                    this.getCacheToken();
                case 400:
                    throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorEmailFormat, "error");
                case 401:
                    throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorUnauthorized, "error");
                case 409:
                    throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorEmailNotFound, "error");
                case 500:
                    throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorSubmited, "error");
            }
        }
        catch (error)
        {
            console.error(this.state.langs[language].unexpectedError + error);
            throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].unexpectedError + error, "error");
        }

        this.setState({ loading: false });
        button.removeAttribute("disabled");
    }

    render()
    {
        const { language, langs } = this.state;
        return(
            <div className="signup">
                <form id="formRegister" onSubmit={this.handleSubmit}>
                    <label id="lblRegister" htmlFor="chk" aria-hidden="true">{langs[language].registerTitle}</label>
                    <div className="containerRegister">
                        <input minLength="1" maxLength="80" id="txtEmailRegister" type="email" name="emailRegister" placeholder={langs[language].emailInput} required autoComplete='off' style={{color: 'var(--text-black-900)'}} />
                        <input minLength="1" maxLength="45" id="txtSenha" type="password" name="senhaRegister" placeholder={langs[language].passwordInput} required autoComplete='off' style={{color: 'var(--text-black-900)'}} />
                        <i className="fa-solid fa-eye eyeIcon" id="iconEye"></i>
                        <input minLength="1" maxLength="45" id="txtSenhaConfirmed" type="password" name="senhaConfirmRegister" placeholder={langs[language].confirmPasswordInput} required autoComplete='off' style={{color: 'var(--text-black-900)'}}/>
                        <button id="button" className="btn btnRegister">{langs[language].registerTitle}</button>
                    </div>
                </form>
            </div>
        )
    }
}