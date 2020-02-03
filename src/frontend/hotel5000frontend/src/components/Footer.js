import React from "react";
import {FaFacebookSquare, FaPhone, FaEnvelope } from 'react-icons/fa'

var style = {
    position: "fixed",
    textAlign: "center",
    left: "0",
    bottom: "0",
    height: "10%",
    width: "100%",
    borderTop: "1px solid gray"
}

var iconstyle = {
    color: "#23d9d2",
    marginLeft: "20px",
    margibRight: "30px",
    height: "30px",
    width: "30px"
}

function Footer() {
    return(
      <div>
          <div style={style}>
              <div>
                  <h3 style={{color: "#23d9d2"}}>Contact us</h3>
              </div>
              <FaFacebookSquare style={iconstyle}/>
              <FaPhone style={iconstyle}/>
              <FaEnvelope style={iconstyle}/>
          </div>
      </div>
    );
}

export default Footer;