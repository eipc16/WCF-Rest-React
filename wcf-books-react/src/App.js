import React from 'react';
import { Route, Switch } from 'react-router-dom';
import Alert from 'react-s-alert';
import MainPage from "./MainPage";
import BookDetails from './BookDetails';
import './App.css';

// mandatory
import 'react-s-alert/dist/s-alert-default.css';

// optional - you can choose the effect you want
import 'react-s-alert/dist/s-alert-css-effects/slide.css';
import 'react-s-alert/dist/s-alert-css-effects/scale.css';
import 'react-s-alert/dist/s-alert-css-effects/bouncyflip.css';
import 'react-s-alert/dist/s-alert-css-effects/flip.css';
import 'react-s-alert/dist/s-alert-css-effects/genie.css';
import 'react-s-alert/dist/s-alert-css-effects/jelly.css';
import 'react-s-alert/dist/s-alert-css-effects/stackslide.css';

function App() {
  return (
    <div className="App">
          <div className="app-body">
              <Switch>
                  <Route exact path="/" component={MainPage} />
                  <Route path="/book/:book_id" component={BookDetails} />
              </Switch>
          </div>

          <Alert stack={{ limit: 3 }} timeout={3000} position='top-right' effect='stackslide' offset={65} />
    </div>
  );
}

export default App;
