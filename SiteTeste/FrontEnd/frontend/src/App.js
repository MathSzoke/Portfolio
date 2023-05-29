import React, { Component } from 'react';
import './index.css';

import { Layout } from './components/Layout';

export default class App extends Component 
{
  static displayName = App.name;

  render() {
    return (
      <>
          <Layout/>
      </>
    );
  }
}
