import React from "react";
import { Button } from "@fluentui/react-components";
import { useChat } from '../../services/context/ChatContext.js'
import { useTranslation } from "react-i18next";

export const StartChatButton = ({ onStarted }) => {
    const { t } = useTranslation();
    const { hasOpenSession, ensureSession, open } = useChat();
    if (hasOpenSession === true) return null;
    const start = async () => {
        await ensureSession();
        open();
        if (onStarted) onStarted();
    };
    return <Button appearance="primary" onClick={start}>{t("chat.startConversation")}</Button>;
};
