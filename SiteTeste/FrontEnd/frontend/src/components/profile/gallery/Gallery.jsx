import React, { useState, useEffect, forwardRef, useImperativeHandle, useRef } from "react";
import './gallery.css'
import { motion, AnimatePresence } from 'framer-motion';
import { BiArrowBack, BiTrash } from 'react-icons/bi'
import { FaPlus } from 'react-icons/fa'
import { RxReload } from 'react-icons/rx'
import Cookies from 'js-cookie';
import { ImageToCrop, ModalImageToCrop } from '../cropImage/CropImage';
import { notify } from "../../notify/notify";
import { ClipLoader } from 'react-spinners';
import { MdAddPhotoAlternate } from 'react-icons/md'

const langs = 
{
    pt: 
    {
        addMore: "Adicionar mais",
        titleHeader: "Mudar a foto do perfil",
        errorHeader: "Aconteceu um erro",
        successHeader: "Sucesso!",
        unexpectedError:  "Um erro inesperado ocorreu :(",
        errorOnDelete: "Não foi possível deletar sua foto pois não o encontrei em meu banco de dados",
        errorOnAlter: "Não foi possível alterar sua foto pois não encontrei nem um e nem outro no meu banco de dados",
        deleteSuccess: "Sua foto foi deletada com sucesso",
        alterSuccess: "Sua foto foi alterada com sucesso",
        selectOnlyOnePhoto: "Selecione apenas uma foto, safado",
        errorConflict: "Ocorreu um erro inesperado no lado do servidor, por favor aguarde e tente novamente."
    },
    en:
    {
        addMore: "Add more",
        titleHeader: "Change profile picture",
        errorHeader: "An error occurred",
        successHeader: "Success!",
        unexpectedError:  "An unexpected error occurred :(",
        errorOnDelete: "Couldn't delete your photo because I couldn't find it in my database",
        errorOnAlter: "Couldn't alter your photo because I couldn't find any in my database",
        deleteSuccess: "Your photo has been deleted with success",
        alterSuccess: "Your photo has been altered with success",
        selectOnlyOnePhoto: "Select only one photo, shameless",
        errorConflict: "An unexpected server-side error occurred, please wait and try again."
    }
}

const language = Cookies.get('language');
const isLogged = !!Cookies.get('tokenAccess');
const userID = Cookies.get('tokenAccess');

export const ModalGallery = forwardRef((props, ref)=>
{
    const [openGallery, setOpenGallery] = useState(false);

    useImperativeHandle(ref, () => 
    {
        return {
            openModalGallery: () => setOpenGallery(true),
            closeModalGallery: () => setOpenGallery(false)
        }
    });

    return(
        <AnimatePresence>
            {
                openGallery && (
                    <>
                        <motion.div className="modalBackdrop" initial={{ opacity: 0 }} animate={{ opacity: 1, transition: { duration: 0.3 } }} exit={{ opacity: 0 }} onClick={() => setOpenGallery(false)}/>
                        <motion.div className="modalContentWrapperGallery">
                            <motion.div className="modalContentGallery">
                                <div className="galleryHeader">
                                    <header className="gHeader">
                                        <div className="areaBtnBack">
                                            <button className="btn btnBackGallery" onClick={() => setOpenGallery(false)}><BiArrowBack className="btnCloseGallery w25h25" /></button>
                                        </div>
                                        <span className="titleGallery">{langs[language].titleHeader}</span>
                                    </header>
                                </div>
                                { props.children }
                            </motion.div>
                        </motion.div>
                    </>
                )
            }
        </AnimatePresence>
    )
})

export const Gallery = () =>
{
    const modalCropImage = useRef();
    const [images, setImages] = useState([]);
    const [selectedFile, setSelectedFile] = useState(null);
    const [selectedImages, setSelectedImages] = useState([]);
    const [countImages, setCountImages] = useState(0);
    const [isLoadingImages, setIsLoadingImages] = useState(false);
    const headers = {'Content-Type' : 'application/json', 'Access-Control-Allow-Origin' : '*'}

    const getAllPhotos = async () =>
    {
        setIsLoadingImages(true);
        try
        {
            setSelectedImages([]);
            const response = await fetch("https://localhost:44395/SaveImage/GetAllPhotos/" + userID);
            const data = await response.json();

            switch(response.status)
            {
                case 404: 
                    setImages(null); 
                    break;
            }
            if(images.length > 1) setImages([]);
            
            setImages(data);

            setCountImages(data.length);
        }
        catch(err)
        {
            notify("errorHeader", err, "error");
        }
        setIsLoadingImages(false);
    }
    
    const handleClick = async (image) => 
    {
        try 
        {
            setSelectedFile(image);

            modalCropImage.current.openModalImageCrop();
        } 
        catch (err) 
        {
            notify("errorHeader", err, "error");
        }
    };
    
    const handleFileChange = (event) => 
    {
        if(countImages != 9)
        {
            setSelectedFile(event.target.files[0]);
            
            let file = event.target.files[0];
            if(!file) return;
    
            if(file.size > 2048000)
            {
                notify(langs[language].errorHeader, langs[language].errorFileSize, 'error');
                setSelectedFile(null);
            }
            else if(file.type !== 'image/jpeg' && file.type !== 'image/png')
            {
                notify(langs[language].errorHeader, langs[language].errorFileType, 'error');
                setSelectedFile(null);
            }
            else
            {
                let reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => {
                    let base64String = reader.result;
                    setSelectedFile(base64String);
                    modalCropImage.current.openModalImageCrop();
                };
                reader.onerror = (error) => {
                    console.log('Error:', error);
                };
            }
        }
        else return;
    };


    const handleSelectImage = (index) => 
    {
        const selectedImagesCopy = [...selectedImages];
        selectedImagesCopy[index] = !selectedImagesCopy[index];
        setSelectedImages(selectedImagesCopy);
    };

    const handleSelectAllImages = (event) =>
    {
        if(document.getElementById("checkAll").classList.contains("someChecked")) setSelectedImages([]) 
        else
        {
            const isChecked = event.target.checked;
        
            const newSelectedImages = images.map(() => isChecked);
        
            setSelectedImages(newSelectedImages);
        }
    };
    
    const getCheckboxClass = () => 
    {
        if (selectedImages.some(value => value)) return selectedImages.every(value => value) === images.every(value => value) ? "allChecked" : "someChecked";

        return "noOneChecked";
    };
    
    const handleDelete = async (urlPhoto) =>
    {
        try
        {
            const encodedUrl = encodeURIComponent(urlPhoto);

            const url = `https://localhost:44395/SaveImage/DeletePhoto/${encodedUrl}`;
            const response = await fetch(url, {
                method: 'DELETE', 
                headers: headers
            });

            switch(response.status)
            {
                case 409:
                    notify(langs[language].errorHeader, langs[language].errorConflict, "error");
                    break;
                case 201:
                    notify(langs[language].successHeader, langs[language].deleteSuccess, "success");
                    break;
            }
        }
        catch(err)
        {
            notify(langs[language].errorHeader, langs[language].unexpectedError + err, "error");
            console.error(err);
        }
    }

    const handleDeleteSelectedImages = async () => 
    {
        selectedImages.forEach((isSelected, index) => 
        {
            if (isSelected) 
            {
                const urlPhotoToDelete = images[index].originalUrl;

                if(urlPhotoToDelete != null || urlPhotoToDelete != undefined)
                {
                    handleDelete(urlPhotoToDelete);
                }
            }
            getAllPhotos();
        });
    };

    const handleAlter = async (urlPhoto) =>
    {
        try
        {
            const encodedUrl = encodeURIComponent(urlPhoto)

            const url = `https://localhost:44395/SaveImage/AlterPhoto/${userID}/${encodedUrl}`

            const response = await fetch(url, { method: 'PUT', headers: headers});

            switch(response.status)
            {
                case 404:
                    notify(langs[language].errorHeader, langs[language].errorOnAlter, "error");
                    break;
                case 409:
                    notify(langs[language].errorHeader, langs[language].errorNotFoundImg, "error");
                    break;
                case 200:
                    notify(langs[language].successHeader, langs[language].alterSuccess, "success");
                    window.location.reload();
                    break;
            }
        }
        catch(e)
        {
            notify(langs[language].errorHeader, langs[language].unexpectedError + e, "error");
            console.error(e);
        }
    }

    const handleAlterSelectedImage = async () =>
    {
        selectedImages.forEach((isSelected, index) => 
        {
            if (isSelected && selectedImages.filter(value => value).length === 1)
            {
                const urlPhotoToDelete = images[index].originalUrl;

                if(urlPhotoToDelete != null || urlPhotoToDelete != undefined)
                {
                    handleAlter(urlPhotoToDelete);
                }
            }
            else
            {
                notify(langs[language].errorHeader ,langs[language].selectOnlyOnePhoto, "error")
            }
        });
    }

    useEffect(() => {
        getAllPhotos();
    }, [])

    return(
        <>
            <div className="contentsGallery">
                {
                    isLoadingImages ? 
                    <div className="isLoadingGallery"><ClipLoader size={'200px'} color="var(--skin-color)" /></div>
                    :
                    <header className="headerPhotos">
                        <div className="optionsPhotos">
                            <div className="inputCheckbox itemsHeaderPhoto">
                                <input id="checkAll" type="checkbox" className={`checkbox checkboxAllPhotos ${getCheckboxClass()}`} onChange={handleSelectAllImages} />
                            </div>
                            <div className="reloadingPhotos itemsHeaderPhoto" onClick={() => getAllPhotos()}>
                                <RxReload className="btnReloadGallery w25h25" />
                            </div>
                        </div>
                        <div className="anotherOptions">
                            <div className="alterPhoto itemsHeaderPhoto" onClick={() => handleAlterSelectedImage()} style={ selectedImages.some(value => value) && selectedImages.filter(value => value).length === 1 ? {visibility: 'visible'} : { visibility: 'hidden' } }>
                                <MdAddPhotoAlternate className="btnAlterPhoto w25h25" />
                            </div>
                            <div className="deletingPhotos itemsHeaderPhoto" onClick={() => handleDeleteSelectedImages()} style={ selectedImages.some(value => value) ? {visibility: 'visible'} : { visibility: 'hidden' } }>
                                <BiTrash className="btnDeletePhoto w25h25" />
                            </div>
                        </div>
                    </header>
                }                
                <div className="divisorGallery marginTop1">
                    <div className="allPhotos">
                        {
                            isLoadingImages ?
                            <div className="isLoadingGallery"><ClipLoader size={'200px'} color="var(--skin-color)" /></div>
                            :
                            images.length > 0 && images.map((image, index) => (
                                <div className={`photo ${selectedImages[index] ? 'selected' : ''}`} key={index}>
                                    <input id={`checkboxPhoto${index}`} type="checkbox" className="checkbox checkboxPhoto" checked={selectedImages[index]} onClick={() => handleSelectImage(index)} />
                                    <div style={{background: 'url(' + image.base64Image + ')', backgroundRepeat: 'round'}} className='images' onClick={ selectedImages.some(value => value) ? () => document.getElementById(`checkboxPhoto${index}`).click() : () => handleClick(image.base64Image)}></div>
                                    <input type="text" style={{display: 'none', visibility: 'hidden'}} value={image.originalUrl} readOnly />
                                </div>
                            ))
                        }
                        {
                            isLoadingImages ? 
                            <div className="isLoadingGallery"><ClipLoader size={'200px'} color="var(--skin-color)" /></div>
                            :
                            <div className="fieldAddPhoto">
                                <button className="btnAddMore" disabled={countImages === 9 ? true : false} style={countImages === 9 ? {cursor: 'no-drop'} : {cursor: 'pointer'}} onClick={() => document.getElementById('fileInput').click()}><p><FaPlus className="w25h25"/></p><p className="txtAddMore">{langs[language].addMore}</p></button>
                            </div>
                        }
                        <input type='file' id='fileInput' max={2048000} accept='image/jpeg, image/png' onChange={handleFileChange} style={{ display: 'none' }} />
                    </div>
                </div>
            </div>
            <div id='CropImage'>
                <ModalImageToCrop ref={modalCropImage}>
                    <ImageToCrop img={selectedFile}></ImageToCrop>
                </ModalImageToCrop>
            </div>
        </>
    )
}

export const ModalCropImage = forwardRef((props, ref) => 
{
    const [openModalCrop, setOpenModalCrop] = useState(false);

    useImperativeHandle(ref, () => {
        return {
            openModalCrop: () => setOpenModalCrop(true),
            closeModalCrop: () => setOpenModalCrop(false)
        }
    });

    return (
        <AnimatePresence>
            { openModalCrop && (
                <>
                    <motion.div className="modalBackdrop" initial={{ opacity: 0 }} animate={{ opacity: 1, transition: { duration: 0.3 } }} exit={{ opacity: 0 }} onClick={() => setOpenModalCrop(false)}/>
                    <motion.div className="modalContentWrapperCrop" initial={{ scale: 0 }} animate={{ scale: 1, transition: { duration: 0.3 } }} exit={{ scale: 0, transition: { delay: 0.1 } }}>
                        <motion.div className="modalContentImageToCrop" initial={{ x: 100, opacity: 0 }} animate={{ x: 0, opacity: 1, transition: { delay: 0.3, duration: 0.3 } }} exit={{ x: 100, opacity: 0, transition: { duration: 0.3 } }}>
                            {props.children}
                        </motion.div>
                    </motion.div>
                </>
            )}
        </AnimatePresence>
    )
})