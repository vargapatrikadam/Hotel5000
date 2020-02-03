import React from 'react';

import Home from './pages/Home';
import NavBar from "./components/NavBar";
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import Lodgings from "./pages/Lodgings";
import Login from './pages/Login';

function App() {
    return(
        <>
            <NavBar/>
            <Router>
                <Switch>
                    <Route exact path="/" component={Home} />
                    <Route exact path="/lodgings" component={Lodgings} />
                    <Route exact path="/login" component={Login} />
                    <Route component={Error} />
                </Switch>
            </Router>

        </>
    );
}

export default App;