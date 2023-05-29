import { React, useEffect, useState, useImperativeHandle, forwardRef, useRef } from 'react'
import { motion, AnimatePresence } from 'framer-motion';
import { FaDesktop, FaCamera, FaTrash, FaPen } from 'react-icons/fa';
import { CgClose } from 'react-icons/cg'
import { MdPhoneIphone } from 'react-icons/md'
import { BiPhotoAlbum } from 'react-icons/bi'
import { BsPersonCircle } from 'react-icons/bs'
import Cookies from 'js-cookie';
import { isMobile } from "react-device-detect";
import { ClipLoader } from 'react-spinners';

import { Login, ModalLogin } from '../profileAccess/login/Login';
import { Register } from '../profileAccess/register/Register';
import { Gallery, ModalGallery } from './gallery/Gallery';

import '../profileAccess/profileAccess.css'

import './profileData.js';
import './profile.css';
import { LiveCamera, ModalLiveCamera } from './camera/Camera'
import { notify } from '../notify/notify';

const langs = 
{
    pt: 
    {
        hello: "Olá,",
        profilePhoto: "Foto do perfil",
        gallery: "Galeria",
        camera: "Câmera",
        change: "Mudar",
        remove: "Remover",
        descCamera: "Uma foto ajuda as pessoas a reconhecerem você e permite que você saiba quando a conta está conectada.",
        errorFileSize: "Tamanho do arquivo excedido, por favor, escolha um arquivo abaixo de 2MB",
        errorFileType: "Tipo de arquivo errado, escolha apenas JPG ou PNG",
        errorHeader: "Erro",
        fileSelected: "Nome do arquivo: ",
        makeLogin: "Acessar",
        nameNotFound: "Parece que não foi possível capturar o seu nome no sistema! Por favor, tente novamente.",
        unexpectedError: "Ocorreu um erro inesperado: ",
        areYouSure: "Você tem certeza que quer escolher uma dessas ações?",
        btnCancel: "Cancelar",
        btnDelete: "Deletar",
        explainBack: "Voltar para editar foto",
        explainDelete: "Excluir a foto do sistema",
        explainRemove: "Apenas remover a foto do perfil",
        errorOnDelete: "Aconteceu um erro ao deletar inesperado sua foto: ",
        successHeader: "Deletado",
        deleteSuccess: "Sua imagem foi deletada com sucesso",
        removeSuccessHeader: "Imagem removida!",
        removedSuccess: "Sua imagem foi removida com sucesso",
        errorOnRemove: "Parece que você não possui foto para ser removida",
        errorNotFoundImg: "Parece que você não possui foto para ser deletada"
    },
    en:
    {
        hello: "Hello,",
        profilePhoto: "Profile picture",
        gallery: "Gallery",
        camera: "Camera",
        change: "Change",
        remove: "Remove",
        descCamera: "A photo helps people recognize you and lets you know when your account is connected.",
        errorFileSize: "Exceeded file size, please choose a file under 2MB.",
        errorFileType: "File type is wrong, please, choose a just file JPG or PNG.",
        errorHeader: "Error",
        fileSelected: "File name: ",
        makeLogin: "Login",
        nameNotFound: "It looks like we were unable to capture your name in the system! Please try again.",
        unexpectedError: "An error unexpected ocurred: ",
        areYouSure: "Are you sure you want to choose one of these stocks?",
        btnCancel: "Cancel",
        btnDelete: "Delete",
        explainBack: "Back to edit photo",
        explainDelete: "Delete system photo",
        explainRemove: "Just remove the profile picture",
        errorOnDelete: "An unexpected error ocurred when delete your image: ",
        successHeader: "Deleted",
        deleteSuccess: "Your image was successfully deleted",
        removeSuccessHeader: "Image removed!",
        removedSuccess: "Your image is removed successfully",
        errorOnRemove: "It looks like you haven't photo to be removed",
        errorNotFoundImg: "It looks like you haven't photo to be deleted"
    }
}

const language = Cookies.get('language');
const isLogged = !!Cookies.get('tokenAccess');
const userID = Cookies.get('tokenAccess');

export const GetPhotoFETCH = async () =>
{
    try
    {
        const response = await fetch("https://localhost:44395/SaveImage/GetPhoto/" + userID);
        const data = await response.text();

        if(response.status === 409 || response.status === 404) return null;

        return data
    }
    catch(e)
    {
        notify(langs[language].errorHeader, langs[language].unexpectedError + e, "error");
        console.error(e);
    }
}

export function Profile()
{
    const modalRef = useRef();
    const modalRefLogin = useRef();
    const [nameUser, setNameUser] = useState('empty');
    
    const getNameUser = async () =>
    {
        try
        {
            const response = await fetch("https://localhost:44395/UserLogin/GetUserName/" + userID);
            const data = await response.text();
    
            setNameUser(data);
    
            if(response.status === 409)
            {
                this.setState({ loading: false });
                throw new notify(langs[language].errorHeader, langs[language].nameNotFound, "error");
            }
        }
        catch (error) 
        {
            notify(langs[language].errorHeader, langs[language].unexpectedError + error, "error");
            console.error(error);
        }
    }

    useEffect(() => 
    {
        getNameUser();
    }, [0]);

    return (
        <div className="style-switcher-profile profile" id="switchProfile">
            <h4 id="margin10px">{langs[language].hello}</h4>
            {
                isLogged ?
                <button id='btnProfile' onClick={() => modalRef.current.open()} className="profileBtn">{nameUser}</button>
                :
                <button id='btnProfile' onClick={() => modalRefLogin.current.open()} className="profileBtn">{langs[language].makeLogin}</button>
            }
            <>
                <Modal ref={modalRef}><ComponentsProfile nameUser={nameUser} /></Modal>
                <ModalLogin ref={modalRefLogin}><Register/><Login/></ModalLogin>
            </>
        </div>
    );
}

/* MODAL PROFILE */
export const Modal = forwardRef((props, ref) => {
    const [open, setOpen] = useState(false);

    useImperativeHandle(ref, () => {
        return {
            open: () => setOpen(true),
            close: () => setOpen(false)
        }
    });

    return (
        <AnimatePresence>
            { open && (
                <>
                    <motion.div className="modalBackdrop" initial={{ opacity: 0 }} animate={{ opacity: 1, transition: { duration: 0.3 } }} exit={{ opacity: 0 }} onClick={() => setOpen(false)}/>
                    <motion.div className="modalContentWrapper" initial={{ scale: 0 }} animate={{ scale: 1, transition: { duration: 0.3 } }} exit={{ scale: 0, transition: { delay: 0.1 } }}>
                        <motion.div className="modalContent" initial={{ x: 100, opacity: 0 }} animate={{ x: 0, opacity: 1, transition: { delay: 0.3, duration: 0.3 } }} exit={{ x: 100, opacity: 0, transition: { duration: 0.3 } }}>
                            {props.children}
                        </motion.div>
                    </motion.div>
                </>
            )}
        </AnimatePresence>
    )
})


export function ComponentsProfile(props)
{
    const modalImageRef = useRef();
    
    const iconEmptyProfile = <BsPersonCircle className='iconEmptyComponentProfile varColorDark'/>;

    const [nameUser, setNameUser] = useState(props.nameUser);
    const [imgPersonProfile, setImgPersonProfile] = useState(null);

    useEffect(() => 
    {
        const fetchData = async () => 
        {
            const data = await GetPhotoFETCH();
            setImgPersonProfile(data);
        };
    
        fetchData();
    }, []);

    return (
        <div className='modalInfoContents'>
            <div className="morphModalTitle">
            </div>
            <div className="modalContentHeader">
                <div className="divDevice">
                    { isMobile ? <MdPhoneIphone className='deviceImage'/> : <FaDesktop className='deviceImage'/> }
                </div>
                <div className="deviceLabel">
                    <span className='userName'>{nameUser}</span>
                    <span className="deviceStatus">
                        { isLogged ? <div className="onlineIndicator"><span className="blinkOnline"></span></div> : <div className="offlineIndicator"><span className="blinkOffline"></span></div> }
                        <span className="online-text">{ isLogged ? "Online" : "Offline" }</span>
                    </span>
                </div>
                <div className="divPerson">
                    <div className="imgDivPerson">
                        <div className="divImg" id='divImgProfile'>
                            { imgPersonProfile === undefined || imgPersonProfile === null || imgPersonProfile === "" ? iconEmptyProfile : <img src={imgPersonProfile} className="img" /> }
                            <button className='buttonImage' onClick={() => modalImageRef.current.openModal()}><FaCamera/></button>
                        </div>
                    </div>
                </div>
            </div>
            <div id='ModalProfile'>
                <ModalImage ref={modalImageRef}><ImgProfilePerson img={imgPersonProfile}/></ModalImage>
            </div>
        </div>
    );
}

/* END MODAL PROFILE */

/* MODAL IMAGE PROFILE */
export const ModalImage = forwardRef((props, ref) => {

    const modalLiveCameraPhoto = useRef();

    const modalConfirmation = useRef();

    const modalGallery = useRef();

    const [openModal, setOpenModal] = useState(false);

    useImperativeHandle(ref, () => {
        return {
            openModal: () => setOpenModal(true),
            closeModal: () => setOpenModal(false)
        }
    });

    return (
        <AnimatePresence>
            { openModal && (
                <>
                    <motion.div className="modalBackdropImage" initial={{ opacity: 0 }} animate={{ opacity: 1, transition: { duration: 0.3 } }} exit={{ opacity: 0 }} onClick={() => setOpenModal(false)}/>
                    <motion.div className="modalContentCropImage" initial={{ scale: 0 }} animate={{ scale: 1, transition: { duration: 0.3 } }} exit={{ scale: 0, transition: { delay: 0.1 } }}>
                        <motion.div className="modalContentPerson" initial={{ x: 100, opacity: 0 }} animate={{ x: 0, opacity: 1, transition: { delay: 0.3, duration: 0.3 } }} exit={{ x: 100, opacity: 0, transition: { duration: 0.3 } }}>                            
                            <main className='mainModal'>
                                <header className="morphModalHeader">
                                    <button className="closeModalImage" onClick={() => setOpenModal(false)}><CgClose className='varColorDark' style={{width: "30px", height: "30px"}}/></button>
                                    <div className="textHeaderPhoto">{langs[language].profilePhoto}</div>
                                </header>
                                {props.children}
                                <div className="buttonsProfile">
                                    <div className="selectionPartChange" style={{flexDirection: "column-reverse"}}>
                                        <div className="buttonChange">
                                            <button className="changeImg" style={{style: "width: 70%"}}><FaPen/> {langs[language].change}</button>
                                            <div className="optionsChangePhoto">
                                                <button className="photoGallery" onClick={() => modalGallery.current.openModalGallery()}><BiPhotoAlbum/> {langs[language].gallery}</button>
                                                <button className="photoCamera" onClick={() => modalLiveCameraPhoto.current.openModalLivePhoto()}><FaCamera/> {langs[language].camera}</button>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="buttonRemoveImg">
                                        <button className="buttonRemove" onClick={() => modalConfirmation.current.openModalConfirmation()}><FaTrash/> {langs[language].remove}</button>
                                    </div>
                                </div>
                            </main>
                        </motion.div>
                    </motion.div>
                    <div id='AppCamera'>
                        <LiveCamera ref={modalLiveCameraPhoto}><ModalLiveCamera/></LiveCamera>
                    </div>
                    <ModalConfirmDelete ref={modalConfirmation}></ModalConfirmDelete>
                    <ModalGallery ref={modalGallery}><Gallery /></ModalGallery>
                </>
            )}
        </AnimatePresence>
    )
})

export function ImgProfilePerson(props)
{
    const iconEmptyProfile = <BsPersonCircle className='iconEmptyProfile varColorDark'/>
    
    const [imgPersonProfile, setImgPersonProfile] = useState(props.img);
    
    return (
        <div className="imgInfoProfile">
            <div className="explainInfoImg">
                {langs[language].descCamera}
            </div>
            <div className="line"></div>
            <div className="locationImageProfile">
                { imgPersonProfile === undefined || imgPersonProfile === null || imgPersonProfile === "" ? iconEmptyProfile : <img src={imgPersonProfile} className="photageProfile" alt='profileImg' /> }
            </div>
        </div>
    )
}

/* END MODAL IMAGE PROFILE */

export const ModalConfirmDelete = forwardRef((props, ref) => 
{
    const [openModalConfirmation, setOpenModalConfirmation] = useState(false);
    const headers = {'Content-Type' : 'application/json', 'Access-Control-Allow-Origin' : '*'}

    useImperativeHandle(ref, () => {
        return {
            openModalConfirmation: () => setOpenModalConfirmation(true),
            closeModalConfirmation: () => setOpenModalConfirmation(false)
        }
    });

    const [urlPhoto, setUrlPhoto] = useState(null);

    useEffect(() => 
    {
        const fetchData = async () => 
        {
            const data = await GetPhotoFETCH();
            setUrlPhoto(data);
        };
    
        fetchData();
    }, []);

    const handleDelete = async () =>
    {
        try
        {
            const encodedUrl = encodeURIComponent(urlPhoto);
            const url = `https://localhost:44395/SaveImage/DeletePhoto/${encodedUrl}`;
            const response = await fetch(url, {
                method: 'DELETE', 
                headers: { headers }
            });

            switch(response.status)
            {
                case 404:
                    notify(langs[language].errorHeader, langs[language].errorOnDelete, "error");
                    break;
                case 409:
                    notify(langs[language].errorHeader, langs[language].errorNotFoundImg, "error");
                    break;
                case 201:
                    notify(langs[language].successHeader, langs[language].deleteSuccess, "success");
                    window.location.reload();
                    break;
            }
        }
        catch(err)
        {
            notify(langs[language].errorHeader, langs[language].unexpectedError + err, "error");
            console.error(err);
        }
    }

    const handleRemove = async () =>
    {
        try
        {
            const url = `https://localhost:44395/SaveImage/RemovePhoto/${userID}`;
            const response = await fetch(url, {
                method: 'PUT', 
                headers: { headers }
            });
            
            if(response.status === 404) notify(langs[language].errorHeader, langs[language].errorOnRemove, "error");
            if(response.status === 204) 
            {
                notify(langs[language].removeSuccessHeader, langs[language].removedSuccess, "success");
                window.location.reload();
            }
        }
        catch(err)
        {
            notify(langs[language].errorHeader, langs[language].unexpectedError + err, "error");
            console.error(err);
        }
    }

    return (
        <AnimatePresence>
            { openModalConfirmation && (
                <>
                    <motion.div className="modalBackdropNoBackground" initial={{ opacity: 0 }} animate={{ opacity: 1, transition: { duration: 0.3 } }} exit={{ opacity: 0 }} onClick={() => setOpenModalConfirmation(false)}/>
                    <motion.div className="modalContentWrapperConfirmDelete" initial={{ scale: 0 }} animate={{ scale: 1, transition: { duration: 0.3 } }} exit={{ scale: 0, transition: { delay: 0.1 } }}>
                        <motion.div className="modalContentConfirmDelete" initial={{ x: 100, opacity: 0 }} animate={{ x: 0, opacity: 1, transition: { delay: 0.3, duration: 0.3 } }} exit={{ x: 100, opacity: 0, transition: { duration: 0.3 } }}>
                            <div className='contentsDelete'>
                                <div className='areYouSure'>
                                    <span>{langs[language].areYouSure}</span>
                                </div>
                                <div className='contentsButtons'>
                                    <div className='contentCancelBtn'>
                                        <span>{langs[language].explainBack}</span>
                                        <button className='btn' onClick={() => setOpenModalConfirmation(false)}>{langs[language].btnCancel}</button>
                                    </div>
                                    <div className='contentDeleteBtn'>
                                        <span>{langs[language].explainDelete}</span>
                                        <button className='btn btnDelete' onClick={() => handleDelete()}>{langs[language].btnDelete}</button>
                                    </div>
                                    <div className='contentRemoveBtn'>
                                        <span>{langs[language].explainRemove}</span>
                                        <button className='btn btnRemove' onClick={() => handleRemove()}>{langs[language].remove}</button>
                                    </div>
                                </div>
                            </div>
                        </motion.div>
                    </motion.div>
                </>
            )}
        </AnimatePresence>
    )
})