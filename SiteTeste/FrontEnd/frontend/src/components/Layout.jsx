import React, { Component } from 'react';

import { NavMenu } from './nav/NavMenu';
import { Switcher } from './switcher/Switcher.jsx';
import { Home } from "./home/Home";
import { About } from "./about/About";
import { Service } from "./service/Service";
import { Portfolio } from "./portfolio/Portfolio";
import { Contact } from "./contact/Contact";
import { Profile } from './profile/Profile.jsx';

export class Layout extends Component
{
    static displayName = Layout.name;

    render()
    {
        return (
            <div>
                <div className='containerSwitcher'>
                    <Profile/>
                    <Switcher />
                </div>
                <NavMenu />
                <Home/>
                <About/>
                <Service/>
                <Portfolio />
                <Contact />
            </div>
        );
    }
}
