import React, { useState } from 'react';
import './NavMenu.css';
import { FaHome, FaUser, FaList, FaBriefcase, FaComments } from 'react-icons/fa';

import './changeSections'; // Importing file js for toggle sections

export const NavMenu = () => {
    const [activeNav, setActiveNav] = useState('#')
    return (
        <nav className="nav" id="aside">
            <li><a href="#home" onClick={() => setActiveNav('#')} className={activeNav === '#' ? 'active' : ''} title="Home"><FaHome /></a></li>
            <li><a href="#about" onClick={() => setActiveNav('#about')} className={activeNav === '#about' ? 'active' : '#'} title="About"><FaUser /></a></li>
            <li><a href="#services" onClick={() => setActiveNav('#services')} className={activeNav === '#services' ? 'active' : '#'} title="Services"><FaList /></a></li>
            <li><a href="#portfolio" onClick={() => setActiveNav('#portfolio')} className={activeNav === '#portfolio' ? 'active' : '#'} title="Portfolio"><FaBriefcase /></a></li>
            <li><a href="#contact" onClick={() => setActiveNav('#contact')} className={activeNav === '#contact' ? 'active' : '#'} title="Contact"><FaComments /></a></li>
        </nav>
    )
}
