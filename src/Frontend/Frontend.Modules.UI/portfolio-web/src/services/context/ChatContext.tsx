import React, { createContext, useContext, useEffect, useMemo, useRef, useState } from "react";
import { Toaster, useToastController } from "@fluentui/react-components";
import { getChatConnection } from "../../services/Hubs/chatHub";
import type { ChatMessageDto } from "../../services/Hubs/chatHub";
import useApiClient from "../useApiClient";

type ChatContextType = {
    sessionId: string | null;
    messages: ChatMessageDto[];
    isOpen: boolean;
    open: () => void;
    close: () => void;
    postMessage: (text: string) => Promise<void>;
    ensureSession: () => Promise<void>;
    hasOpenSession: boolean | null;
};

const ChatContext = createContext<ChatContextType | null>(null);

export const ChatProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const api = useApiClient();
    const { dispatchToast } = useToastController();
    const [sessionId, setSessionId] = useState<string | null>(null);
    const [messages, setMessages] = useState<ChatMessageDto[]>([]);
    const [isOpen, setIsOpen] = useState(false);
    const [hasOpenSession, setHasOpenSession] = useState<boolean | null>(null);
    const openRef = useRef(false);

    useEffect(() => {
        api.get("/api/v1/chat/sessions/me/has-open", { skipAuth: true })
            .then((b: any) => setHasOpenSession(Boolean(b)))
            .catch(() => setHasOpenSession(null));
    }, [api]);

    useEffect(() => {
        const c = getChatConnection();
        c.off("message");
        c.on("message", (m: ChatMessageDto) => {
            setMessages(prev => [...prev, m]);
            if (!openRef.current) dispatchToast("Nova mensagem recebida", { intent: "info" });
        });
        if (c.state !== "Connected" && c.state !== "Connecting") {
            c.start().catch(() => { });
        }
        return () => {
            c.off("message");
            c.stop().catch(() => { });
        };
    }, [dispatchToast]);

    const fetchMessages = async (sid?: string) => {
        const id = sid ?? sessionId;
        if (!id) return;
        const list = await api.get(`/api/v1/chat/sessions/${id}/messages`, { skipAuth: true });
        setMessages(Array.isArray(list) ? list : []);
    };

    useEffect(() => {
        if (!sessionId || !isOpen) return;
        fetchMessages().catch(() => { });
        const h = setInterval(() => fetchMessages().catch(() => { }), 1500);
        return () => clearInterval(h);
    }, [sessionId, isOpen]);

    const ensureSession = async (recipientId?: string) => {
        if (sessionId) return;
        const data = await api.post("/api/v1/chat/sessions/start", { RecipientId: recipientId });
        setSessionId(data.id);
        await fetchMessages(data.id);
    };

    const postMessage = async (text: string) => {
        if (!sessionId) return;
        const m = await api.post(`/api/v1/chat/sessions/${sessionId}/messages`, { content: text, asMe: false });
        setMessages(prev => [...prev, { id: m.id, sessionId: m.sessionId, sender: m.sender, content: m.content, createdAt: m.createdAt }]);
        await fetchMessages();
    };

    const open = () => {
        openRef.current = true;
        setIsOpen(true);
    };

    const close = () => {
        openRef.current = false;
        setIsOpen(false);
    };

    const value = useMemo(
        () => ({ sessionId, messages, isOpen, open, close, postMessage, ensureSession, hasOpenSession }),
        [sessionId, messages, isOpen, hasOpenSession]
    );

    return (
        <ChatContext.Provider value={value}>
            <Toaster />
            {children}
        </ChatContext.Provider>
    );
};

export const useChat = () => {
    const ctx = useContext(ChatContext);
    if (!ctx) throw new Error("ChatContext");
    return ctx;
};
