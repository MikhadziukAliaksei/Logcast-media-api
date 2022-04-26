import * as React from "react";
import Image from 'react-bootstrap/Image';
import {useLogoStyles} from "./logo.styles";

export const Logo = () => {

    const logoStyle = useLogoStyles();

    return (
        <a href={"/"}>
            <Image src="./images/logcast_logo.png" alt="LogCast Logo" style={logoStyle}/>
        </a>
    );
};