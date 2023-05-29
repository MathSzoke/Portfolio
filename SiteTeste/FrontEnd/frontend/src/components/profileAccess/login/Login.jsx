import { AnimatePresence, motion } from "framer-motion";
import React, { useImperativeHandle, useState, forwardRef, Component } from "react";
import './login.css';
import Cookies from 'js-cookie';
import { ClipLoader } from 'react-spinners';
import { notify } from '../../notify/notify';
import { ForgotPass } from "../Forgot/ForgotPass";

const language = Cookies.get('language');

export const ModalLogin = forwardRef((props, ref)=>
{
    const [openLogin, setOpenLogin] = useState(false);

    useImperativeHandle(ref, () => 
    {
        return {
            open: () => setOpenLogin(true),
            close: () => setOpenLogin(false)
        }
    });

    return (
        <AnimatePresence>
            {
                openLogin && (
                    <>
                        <motion.div className="modalBackdropSignUp" initial={{ opacity: 0 }} animate={{ opacity: 1, transition: { duration: 0.3 } }} exit={{ opacity: 0 }} onClick={() => setOpenLogin(false)}/>
                            <div id="main" className="main active-sx">
                                <input type="checkbox" id="chk" aria-hidden="true" />
                                {props.children}
                            </div>
                            <ForgotPass></ForgotPass>
                    </>
                )
            }
        </AnimatePresence>
    )
})

export class Login extends Component
{
    handleForgotPass = () =>
    {
        document.getElementById('fgPass').classList.remove('inactive-dx');
        document.getElementById('main').classList.remove('active-sx');
        document.getElementById('fgPass').classList.add('active-dx');
        document.getElementById('main').classList.add('inactive-sx');
    }

    state = {
        language: 'en',
        loading: false,
        langs: 
        {
            pt: 
            {
                emailInput: "Email",
                passwordInput: "Senha",
                loginMessage: "Acessar",
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
                sendSuccessfully: "Login sucedido"
            },
            en:
            {
                emailInput: "Email",
                passwordInput: "Password",
                loginMessage: "Login",
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
                sendSuccessfully: "Successfully login"
            },
        },
    }

    getCacheToken = async () => 
    {
        try
        {
            const email = this.emailInput.value;
            const response = await fetch("https://localhost:44395/UserLogin/GetUserID/" + email);
            const data = await response.text();
            Cookies.set('tokenAccess', data);

            if(response.status === 409)
            {
                this.setState({ loading: false });
                throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorEmailNotFound, "error");
            }
        }
        catch (error) 
        {
            notify(this.state.langs[language].errorHeader, this.state.langs[language].errorEmailNotFound, "error");
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
            const response = await fetch('https://localhost:44395/UserLogin/PostUserLogin', {
                method: 'POST',
                statusCode: 201,
                body: JSON.stringify(formData),
                headers: headers
            });

            if (response.ok)
            {
                notify(this.state.langs[language].sendHeader, this.state.langs[language].sendSuccessfully, "success");
                this.getCacheToken();
                window.location.reload();
            }
            else if(response.status === 400)
            {
                notify(this.state.langs[language].errorHeader, this.state.langs[language].errorEmailFormat, "error");
            }
            else if(response.status === 401)
            {
                notify(this.state.langs[language].errorHeader, this.state.langs[language].errorUnauthorized, "error");
            }
            else if(response.status === 409)
            {
                throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorEmailNotFound, "error");
            }
            else 
            {
                throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].errorSubmited, "error");
            }
        }
        catch (error)
        {
            console.error(this.state.langs[language].unexpectedError + error);
        }

        this.setState({ loading: false });
        button.removeAttribute("disabled");
    }

    render()
    {
        const { language, langs } = this.state;
        return(
            <div className="login">
                <form id="formLogin" onSubmit={this.handleSubmit}>
                    <label id="lblLogin" htmlFor="chk" aria-hidden="true">Login</label>
                    <div className="containerLogin">
                        <input className="form-control" type="email" name="emailLogin" id="email" placeholder={langs[language].emailInput} required="" maxLength="80" ref={input => this.emailInput = input} style={{color: 'black'}} />
                        <input className="form-control" type="password" name="passLogin" id="pass" placeholder={langs[language].passwordInput} required="" maxLength="80" ref={input => this.passwordInput = input} style={{color: 'black'}} />
                        <i className="fa-solid fa-eye eyeIconLogin" id="iconEyeLogin"></i>
                        <button id="submitLogin" type="submit" className="btn btnLogin">{ this.state.loading ? <ClipLoader /> : this.state.langs[language].loginMessage }</button>
                    </div>
                </form>
                <button id="frgPass" className="btnForgotPass sx btn" onClick={this.handleForgotPass}>{langs[language].changePass}</button>
            </div>
        )
    }
}