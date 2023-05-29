import React, { useState, useImperativeHandle, forwardRef, useEffect, Component, useContext } from 'react'
import { motion, AnimatePresence } from 'framer-motion';
import { CgClose } from 'react-icons/cg'
import './cropImg.css'
import 'tui-image-editor/dist/tui-image-editor.css';
import Cookies from 'js-cookie';
import { notify } from '../../notify/notify';
import ImageEditor from '@toast-ui/react-image-editor'

const userID = Cookies.get('tokenAccess');

export const ModalImageToCrop = forwardRef((props, ref) => 
{
    const [openModalImageCrop, setOpenModalImageCrop] = useState(false);

    useImperativeHandle(ref, () => {
        return {
            openModalImageCrop: () => setOpenModalImageCrop(true),
            closeModalImageCrop: () => setOpenModalImageCrop(false)
        }
    });

    return (
        <AnimatePresence>
            { openModalImageCrop && (
                <>
                    <motion.div className="modalBackdropCamera" initial={{ opacity: 0 }} animate={{ opacity: 1, transition: { duration: 0.3 } }} exit={{ opacity: 0 }} onClick={() => setOpenModalImageCrop(false)}/>
                    <motion.div className="modalContentWrapperCrop" initial={{ scale: 0 }} animate={{ scale: 1, transition: { duration: 0.3 } }} exit={{ scale: 0, transition: { delay: 0.1 } }}>
                        <motion.div className="modalContentCrop" initial={{ x: 100, opacity: 0 }} animate={{ x: 0, opacity: 1, transition: { delay: 0.3, duration: 0.3 } }} exit={{ x: 100, opacity: 0, transition: { duration: 0.3 } }}>                            
                            <header className="modalCropHeader">
                                <button className="closeModalCrop" onClick={() => setOpenModalImageCrop(false)}><CgClose style={{width: "20px", height: "30px"}}/></button>
                            </header>
                            {props.children}
                        </motion.div>
                    </motion.div>
                </>
            )}
        </AnimatePresence>
    )
})

export class ImageToCrop extends Component
{
    constructor(props)
    {
        super(props);

        this.state = {
            language: 'en',
            langs: 
            {
                pt: 
                {
                    buttonSave: "Salvar",
                    successHeader: "Salvo",
                    imgSavedSuccess: "Imagem salva com sucesso!",
                    errorHeader: "Erro",
                    imgSavedError: "Ocorreu um erro enquanto salvamos a imagem",
                    unexpectedError: "Ocorreu um erro inesperado "
                },
                en:
                {
                    buttonSave: "Save",
                    successHeader: "Saved",
                    imgSavedSuccess: "Image saved successfully!",
                    errorHeader: "Error",
                    imgSavedError: "An error occurred while saving the image",
                    unexpectedError: "An unexpected error ocurred "
                },
            },
        }
        this.img = props.img;
    }

    editorRef = React.createRef();

    handleClickButton = async () => {
        const language = Cookies.get('language');

        const editorInstance = this.editorRef.current.getInstance();

        const dataURL = editorInstance.toDataURL('png');
        const base64URL = dataURL.split(',')[1];

        const dataJson = {
            userID: userID,
            urlPhoto: base64URL
        }

        const headers = {'Content-Type' : 'application/json', 'Access-Control-Allow-Origin' : '*'};

        try
        {
            const response = await fetch('https://localhost:44395/SaveImage/PostSaveImage', {
                method: 'POST',
                statusCode: 201,
                body: JSON.stringify(dataJson),
                headers: headers
            });

            if (response.ok)
            {
                notify(this.state.langs[language].successHeader, this.state.langs[language].imgSavedSuccess, "success");
                window.location.reload();
            }
            else 
            {
                throw new notify(this.state.langs[language].errorHeader, this.state.langs[language].imgSavedError, "error");
            }
        }
        catch (error)
        {
            notify(this.state.langs[language].errorHeader, this.state.langs[language].unexpectedError + error, "error");
        }
    };

    render()
    {
        const language = Cookies.get('language');
        const divBeforeBtn = document.createElement("div");
        divBeforeBtn.classList.add("tui-btn-save-img");

        const button = document.createElement('button');
        button.textContent = this.state.langs[language].buttonSave;
        button.classList.add("btnSave");
        button.onclick = this.handleClickButton;

        setTimeout(() => {
            const imageEditor = document.querySelector('.tui-image-editor-header-buttons');
            if (!imageEditor) return;
            
            const existingButton = document.querySelector('.tui-btn-save-img');

            if (existingButton) return;

            imageEditor.appendChild(divBeforeBtn);

            divBeforeBtn.appendChild(button);
        }, 100);

        return (
            <>
                <div className="contentsToCrop">
                    <div className='containerProp'>
                        <ImageEditor
                            ref={this.editorRef}
                            includeUI={{
                                loadImage: {
                                    path: this.img,
                                    name: 'SampleImage',
                                },
                                menu: ['crop', 'flip', 'rotate', 'draw', 'shape', 'icon', 'text', 'filter'],
                                uiSize: {
                                    width: '100%',
                                    height: '100%'
                                },
                                menuBarPosition: 'left'
                            }}
                        />
                    </div>
                </div>
            </>
        );
    }
}

