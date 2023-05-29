import React from 'react'

import { BiExpand } from 'react-icons/bi'

export const ProjectsCards = ({ project, handleOpen }) =>
{
    return (
        <div className="portfolio-item-inner shadow-dark">
            <div className="portfolio-img">
                <iframe src={project.baseUrl} frameBorder="0" className='urlProject' id={project.idIframe}></iframe>
            </div>
            <h2 className="portfolio-title">{ project.title }
                <button className='btnBeforeIcon' onClick={() => handleOpen(project.id) }><BiExpand className='expansiveWindow'/></button>
            </h2>
        </div>
    )
}