import { React, useEffect, useState, useImperativeHandle, forwardRef, useRef } from 'react'
import { CgClose } from 'react-icons/cg'
import { FaCamera } from 'react-icons/fa'
import { motion, AnimatePresence } from 'framer-motion';
import Cookies from 'js-cookie';

import { notify } from '../../notify/notify.js'
import './camera.css';

const language = Cookies.get('language');

const langs = 
{
    pt: 
    {
        errorCamera: "Não foi encontrada nenhuma câmera disponivel no momento",
        errorHeader: "Erro"
    },
    en:
    {
        errorCamera: "No available camera found at this time",
        errorHeader: "Error"
    }
}

/* MODAL PHOTAGE LIVE */

export const LiveCamera = forwardRef((props, ref) => {
    const [openModalLivePhoto, setOpenModalLivePhoto] = useState(false);

    useImperativeHandle(ref, () => {
        return {
            openModalLivePhoto: () => setOpenModalLivePhoto(true),
            closeModalLivePhoto: () => setOpenModalLivePhoto(false)
        }
    });

    return (
        <AnimatePresence>
            { openModalLivePhoto && (
                <>
                    <motion.div className="modalBackdropCamera" initial={{ opacity: 0 }} animate={{ opacity: 1, transition: { duration: 0.3 } }} exit={{ opacity: 0 }} onClick={() => setOpenModalLivePhoto(false) }/>
                    <motion.div className="modalContentWrapperCamera" initial={{ scale: 0 }} animate={{ scale: 1, transition: { duration: 0.3 } }} exit={{ scale: 0, transition: { delay: 0.1 } }}>
                        <motion.div className="modalContentCamera" initial={{ x: 100, opacity: 0 }} animate={{ x: 0, opacity: 1, transition: { delay: 0.3, duration: 0.3 } }} exit={{ x: 100, opacity: 0, transition: { duration: 0.3 } }}>                            
                            <header className="modalCameraHeader">
                                <button className="closeModalCamera" onClick={() => setOpenModalLivePhoto(false)}><CgClose style={{width: "20px", height: "30px"}}/></button>
                            </header>
                            {props.children}
                        </motion.div>
                    </motion.div>
                </>
            )}
        </AnimatePresence>
    )
})

export function ModalLiveCamera()
{
    const videoRef = useRef(null);
    const photoRef = useRef(null);

    const [isVideo, setIsVideo] = useState(false);
    const [hasPhoto, setHasPhoto] = useState(false);

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

    const getVideo = () => 
    {
        navigator.mediaDevices.getUserMedia({
            video: { width: 1920, height: 1080 }
        }).
        then
        (stream =>
        {
            let video = videoRef.current;
            video.srcObject = stream;
            video.play();
            setIsVideo(true);
        }).
        catch(err => 
        {
            setIsVideo(false);
            notify(langs[language].errorHeader, langs[language].errorCamera, 'error');
        })
    }

    useEffect(() => { getVideo(); }, [videoRef]);

    return (
        <div className="modalCamera">
            <div className="camera">
                <video className='videoCamera' ref={videoRef}></video>
                { isVideo ? <button className="btnSnapPhoto" onClick={() => SnapPhotage(hasPhoto, photoRef)}><FaCamera style={{width: "2em", height: "2em"}}/></button> : null }
            </div>
        </div>
    )
}

export function SnapPhotage(hasPhoto, photoRef)
{
    return(
        <div className={"result" + (hasPhoto ? 'hasPhoto' : '')}>
            <canvas ref={photoRef}></canvas>
        </div>
    )
}

/* END MODAL PHOTAGE LIVE */
