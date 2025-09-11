import { useState, useRef } from "react";
import { Button, makeStyles, tokens } from "@fluentui/react-components";
import { SendFilled, Emoji24Regular } from "@fluentui/react-icons";
import EmojiPicker from "emoji-picker-react";
import {useTranslation} from "react-i18next";

const useStyles = makeStyles({
    wrapper: {
        position: "relative",
        width: "100%",
        display: "flex",
        alignItems: "center",
        background: tokens.colorNeutralBackground1,
        borderRadius: "26px",
        gap: '5px',
        padding: "8px",
        minHeight: "56px",
        maxHeight: "180px",
        boxSizing: "border-box",
        transition: "box-shadow 0.2s",
    },
    textarea: {
        width: "100%",
        minHeight: "36px",
        maxHeight: "110px",
        resize: "none",
        border: "none",
        outline: "none",
        fontSize: ".9375rem",
        background: "transparent",
        color: tokens.colorNeutralForeground1,
        paddingRight: "40px",
        paddingLeft: "20px",
        overflowY: "auto",
        lineHeight: "1.6",
        boxSizing: "border-box",
        fontFamily: 'sans-serif'
    },
    sendBtn: {
        background: tokens.colorBrandBackground,
        color: "#fff",
        borderRadius: "50%",
        minWidth: "36px",
        minHeight: "36px",
        boxShadow: "0 2px 12px #0001",
        zIndex: 1,
        "&:hover": {
            background: tokens.colorBrandBackgroundHover,
        }
    },
});

export function ChatInput({ value, onChange, onSend }) {
    const s = useStyles();
    const textareaRef = useRef(null);
    const { t } = useTranslation();

    const handleInput = (e) => {
        const el = textareaRef.current;
        if (!el) return;
        el.style.height = "36px";
        el.style.height = Math.min(el.scrollHeight, 110) + "px";
        if (onChange) onChange(e.target.value);
    };

    const handleKeyDown = (e) => {
        if (e.key === "Enter" && !e.shiftKey) {
            e.preventDefault();
            if (value.trim()) {
                onSend(value);
            }
        }
    };

    return (
        <div className={s.wrapper}>
            <textarea
                ref={textareaRef}
                className={s.textarea}
                value={value}
                onChange={(e) => {
                    handleInput(e);
                }}
                onInput={handleInput}
                onKeyDown={handleKeyDown}
                placeholder={t('chat.inputPlaceholder')}
                rows={1}
                style={{
                    maxHeight: "110px",
                }}
            />
            <Button
                type="button"
                icon={<SendFilled />}
                className={s.sendBtn}
                disabled={!value.trim()}
                onClick={() => {
                    if (value.trim()) onSend(value);
                }}
            />
        </div>
    );
}
