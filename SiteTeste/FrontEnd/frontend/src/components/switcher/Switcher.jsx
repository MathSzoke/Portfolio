// react imports
import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import Cookies from 'js-cookie';

// icons imports
import { FaCog, FaSun, FaMoon, FaWindowClose } from 'react-icons/fa';
import { ImExit } from 'react-icons/im';
import { US, BR } from 'country-flag-icons/react/3x2'

// functions imports
import { openSwitch, closeSwitch, setActiveStyle } from './openSwitch.js';

// files imports
import './switcher.css';
import { openSwitchProfile, closeSwitchProfile } from '../profile/profileData';
import { notify } from '../notify/notify.js';

export function Switcher ()
{
  const bodyDark = document.body.classList.contains('dark');
  const [iconThemeColor, setIconThemeColor] = useState(null);

  const colorTheme = Cookies.get('colorTheme');

  useEffect(() => 
  {
    if (colorTheme === 'dark') 
    {
      document.body.classList.add('dark');
      setIconThemeColor(<FaSun />);
    } 
    else 
    {
      document.body.classList.remove('dark');
      setIconThemeColor(<FaMoon />);
    }
  }, [setIconThemeColor]);
  
  const darkLightMode = () => 
  {
    document.body.classList.toggle('dark');
    const colorTheme = document.body.classList.contains('dark') ? 'dark' : 'light';
    Cookies.set('colorTheme', colorTheme);
    setIconThemeColor(document.body.classList.contains('dark') ? <FaSun /> : <FaMoon />);
  };

  useEffect(() => 
  {
    if (bodyDark) 
    {
      setIconThemeColor(<FaSun />);
    } 
    else 
    {
      setIconThemeColor(<FaMoon />);
    }
  }, [bodyDark]);

  const langs = 
  {
    pt: 
    {
      color: "Cores dos destaques"
    },
    en:
    {
      color: "Highlight Colors"
    }
  }

  const language = Cookies.get('language');
  const isLogged = !!Cookies.get('tokenAccess');
  const userID = Cookies.get('tokenAccess');
  
  const handleExitLogin = async () =>
  {
    try
    {
        const response = await fetch("https://localhost:44395/UserLogin/GetLogout/" + userID);

        switch(response.status)
        {
          case 409:
            this.setState({ loading: false });
            notify(this.state.langs[language].errorHeader, this.state.langs[language].errorLogout, "error");
            break;
          case 200:
            Cookies.remove('tokenAccess');
            window.location.reload();
            break;
        }
    }
    catch (error) 
    {
        notify(this.state.langs[language].errorHeader, this.state.langs[language].errorLogout, "error");
        console.error(error);
    }
  }

  return (
      <>
        <div className="style-switcher" id="switch">
            <div className="style-switcher-toggler s-icon" onClick={function(){ openSwitch(); openSwitchProfile() }}>
              <i><FaCog className='rotate'/></i>
            </div>
            <div className="day-night s-icon" onClick={ () => darkLightMode() }>
              <i id="toggleTheme">{ iconThemeColor }</i>
            </div>
            {
              isLogged ? 
              <div className="s-icon logout" onClick={ () => handleExitLogin() }>
                <i id='logout'><ImExit/></i>
              </div>
              :
              null
            }
            <h4>{langs[language].color}</h4>
            <i className="closeSwitcher" onClick={function(){ closeSwitch(); closeSwitchProfile() }}><FaWindowClose/></i>
            <div className="colors">
                <span className="color-5" onClick={setActiveStyle('color-5')}></span>
            </div>
            <div id="slider-horizontal"></div>
        </div>
        <LanguageDropdown />
    </>
  );
}

export function LanguageDropdown() 
{
  const data = [
    {
      value: 1,
      text: 'PT',
      icon: <BR title="Portuguese" className='iconOptions'/>,
      lang: 'pt'
    },
    {
      value: 2,
      text: 'EN',
      icon: <US title="English" className='iconOptions'/>,
      lang: 'en'
    }
  ];

  const language = Cookies.get('language');
  const [selectedOption, setSelectedOption] = useState(data.find(d => d.lang === language) || data[0]);

  const handleChange = e => 
  {
    setSelectedOption(e);
    Cookies.set('language', e.lang);
    window.location.reload();
  }

  useEffect(() => {
    document.querySelector('html').setAttribute('lang', selectedOption.lang);
  }, [selectedOption]);

  return (
    <div className="style-background-language">
      <Select className="dropdown-content" classNamePrefix="react-select" placeholder="Select option" value={selectedOption} options={data} onChange={handleChange} getOptionLabel={e => (
        <div style={{ display: 'flex', alignItems: 'center' }}>
          {e.icon}
          <span style={{ marginLeft: 5 }}>{e.text}</span>
        </div>
      )}/> {/* defaultMenuIsOpen */}
    </div>
  );
}
