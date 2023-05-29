import './forgotPass.css';
import React, { useState } from 'react';
import { MdArrowBackIosNew, MdSend } from 'react-icons/md';

export const ForgotPass = () =>
{
    const [containerClass, setContainerClass] = useState("inactive-dx");
    const [textDisabled, setTextDisabled] = useState(true);
    const [emailText, setEmailText] = useState("");

    const handleForgotPass = () => {
        setContainerClass("active-dx");
        setTextDisabled(false);
    }

    const handleBack = () => {
        document.getElementById('fgPass').classList.remove('active-dx');
        document.getElementById('main').classList.remove('inactive-sx');
        document.getElementById('fgPass').classList.add('inactive-dx');
        document.getElementById('main').classList.add('active-sx');
    }
    return(        
        <div id='fgPass' className={`containerForgotPass ${containerClass}`}>
            <div className="forgotPass">
                <label aria-hidden="true" className='lblRecoverPass'>Recuperar senha</label>
                <div className='divForgotPass'>
                    <input 
                        minLength="1" 
                        maxLength="80" 
                        className="w100" 
                        type="email" 
                        name="emailCodeValidation" 
                        placeholder="Email" 
                        autoComplete='off' 
                        disabled={textDisabled}
                        value={emailText}
                        onChange={e => setEmailText(e.target.value)}
                    />
                    <button className="btnSend dx" onClick={handleForgotPass}><MdSend style={{width: '4em', height: '4em'}}/></button>
                </div>
                <button className="btnBack dx" onClick={handleBack}><MdArrowBackIosNew style={{width: '1.5em', height: '1.5em'}} /></button>
            </div>
        </div>
    )
}