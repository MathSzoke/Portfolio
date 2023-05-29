import React, { useState, useImperativeHandle, forwardRef, useRef } from 'react'
import { motion, AnimatePresence } from 'framer-motion';

export const ProjectsModal = React.forwardRef((props, ref) =>
{
    const [openModalProjects, setOpenModalProjects] = useState(false);

    useImperativeHandle(ref, () => {
        return {
            openModalProjects: () => setOpenModalProjects(true),
            closeModalProjects: () => setOpenModalProjects(false)
        }
    });

    return (
        <AnimatePresence>
            { openModalProjects && (
                <>
                    <motion.div className="modalBackdropProjects" initial={{ opacity: 0 }} animate={{ opacity: 1, transition: { duration: 0.3 } }} exit={{ opacity: 0 }} onClick={() => setOpenModalProjects(false)}/>
                    <motion.div className="modalContentWrapperProjects" initial={{ scale: 0 }} animate={{ scale: 1, transition: { duration: 0.3 } }} exit={{ scale: 0, transition: { delay: 0.1 } }}>
                        <motion.div className="modalContentProjects" initial={{ x: 100, opacity: 0 }} animate={{ x: 0, opacity: 1, transition: { delay: 0.3, duration: 0.3 } }} exit={{ x: 100, opacity: 0, transition: { duration: 0.3 } }}>                            
                            {props.children}
                        </motion.div>
                    </motion.div>
                </>
            )}
        </AnimatePresence>
    )
})

export const ProjectExpanded = forwardRef(({ project }, ref) => 
{
    return(
        <div className='projectInside' ref={ref}>
            <iframe src={project.baseUrl} frameBorder="0" className='urlProject' id='iframe'></iframe>
        </div>
    )
})