import React from "react";
import './Button.css';

export const Button = (props: React.PropsWithChildren<any>) => {

    return <button className={`logcast_button ${props.color}`} {...props}>{props.children}</button>
}