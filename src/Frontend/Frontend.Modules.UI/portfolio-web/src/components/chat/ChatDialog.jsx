import { useEffect, useMemo, useRef, useState } from 'react';
import {
    Button, Persona, makeStyles, tokens, Toaster, useToastController, useId,
    ToastTitle, ToastTrigger, Toast, Link
} from '@fluentui/react-components';
import { Dismiss24Regular } from '@fluentui/react-icons';
import { useAuth } from '../../services/auth';
import { useTranslation } from 'react-i18next';
import { ChatInput } from './ChatInput.jsx';
import { useChat } from '../../services/context/ChatContext';
import { HeaderWithStatus } from './HeaderWithStatus.jsx';
import { StartChatButton } from './StartChatButton.jsx';
import { getOwnerPresence } from '../../services/Hubs/presence';
import getApiClient from '../../services/apiClient';

const useStyles = makeStyles({
    floatingButton: { position: 'fixed', right: '24px', bottom: '24px', zIndex: 9999 },
    container: {
        position: 'fixed', right: '24px', bottom: '88px', zIndex: 9999,
        width: '60%', height: '60%', display: 'flex',
        background: tokens.colorNeutralBackground1,
        border: `1px solid ${tokens.colorNeutralStroke2}`,
        borderRadius: '18px', boxShadow: '0 4px 32px #0002'
    },
    sidebar: {
        width: '35%', background: tokens.colorNeutralBackground2,
        borderTopLeftRadius: '18px', borderBottomLeftRadius: '18px',
        borderRight: `1px solid ${tokens.colorNeutralStroke2}`,
        display: 'flex', flexDirection: 'column', gap: '12px', padding: '20px 12px 12px'
    },
    userCard: { width: '100%', borderRadius: '14px', gap: '12px', display: 'flex', flexDirection: 'column' },
    persona: { margin: '10px' },
    list: { display: 'flex', flexDirection: 'column', gap: '8px', overflowY: 'auto', paddingRight: '4px' },
    listItem: {
        width: '100%', borderRadius: '12px', padding: '0',
        display: 'flex', alignItems: 'center', cursor: 'pointer',
        border: `1px solid ${tokens.colorNeutralStroke2}`, overflow: 'hidden',
        ':hover': { backgroundColor: tokens.colorNeutralBackground2Hover, transform: 'scale(1.02)' }
    },
    listItemBtn: { width: '100%', justifyContent: 'flex-start', padding: '6px 8px', height: 'auto' },
    chatArea: {
        flex: 1, display: 'flex', flexDirection: 'column',
        background: tokens.colorNeutralBackground1Hover,
        borderTopRightRadius: '18px', borderBottomRightRadius: '18px'
    },
    topBar: {
        height: '52px', display: 'flex', alignItems: 'center', justifyContent: 'space-between',
        gap: '12px', padding: '0 10px', borderBottom: `1px solid ${tokens.colorNeutralStroke2}`
    },
    messagesWrapper: {
        flex: 1, overflowY: 'auto', padding: '1em', background: tokens.colorNeutralBackground1Hover,
        display: 'flex', flexDirection: 'column', marginBottom: '40px'
    },
    emptyState: { flex: 1, display: 'flex', alignItems: 'center', justifyContent: 'center' },
    inputBar: { background: tokens.colorNeutralBackground1, borderRadius: '26px', marginRight: '12px', marginLeft: '12px', marginBottom: '12px' }
});

async function fetchSessions() {
    const api = getApiClient();
    const r = await api.get('/api/v1/chat/sessions');
    const items = Array.isArray(r.items) ? r.items : [];
    return items.map(x => ({
        id: x.id ?? x.Id,
        senderId: x.senderId ?? x.SenderId ?? null,
        recipientId: x.recipientId ?? x.RecipientId ?? null,
        status: x.status ?? x.Status,
        createdAt: x.createdAt ?? x.CreatedAt,
        lastSenderSeenAt: x.lastSenderSeenAt ?? x.LastSenderSeenAt ?? null,
        lastRecipientSeenAt: x.lastRecipientSeenAt ?? x.LastRecipientSeenAt ?? null
    }));
}

async function fetchUserMini(userId) {
    const api = getApiClient();
    const r = await api.get(`/api/v1/User/ById/${userId}`);
    return { id: r.id, name: r.fullName, email: r.email, picture: r.avatarUrl };
}

export function ChatDialog({ apiBase, open, onClose, messages, onSend, onStart, started, isOwner = false }) {
    const s = useStyles();
    const [input, setInput] = useState('');
    const endRef = useRef(null);
    const wrapperRef = useRef(null);
    const bubblesRef = useRef(null);
    const { mathInfo, userInfo, token } = useAuth();
    const { t } = useTranslation();

    const { sessionId, ensureSession, selectSession } = useChat();

    const [online, setOnline] = useState(false);
    const [sessions, setSessions] = useState([]);
    const [peers, setPeers] = useState({});
    const [activeSessionId, setActiveSessionId] = useState(null);

    const toasterId = useId('chat-toaster');
    const { dispatchToast, dismissToast, pauseToast } = useToastController(toasterId);
    const stickyRef = useRef(null);

    useEffect(() => {
        if (!apiBase) return;
        getOwnerPresence(apiBase, 'matheusszoke@gmail.com').then(r => setOnline(!!r?.online)).catch(() => setOnline(false));
    }, [apiBase]);

    useEffect(() => {
        if (!open) {
            if (stickyRef.current) {
                dismissToast(stickyRef.current);
                stickyRef.current = null;
            }
            return;
        }
        if (!stickyRef.current && userInfo != null) {
            const id = dispatchToast(
                <Toast>
                    <ToastTitle action={<ToastTrigger><Link>Dismiss</Link></ToastTrigger>}>{t('common.aiWarn')}</ToastTitle>
                </Toast>,
                { toastId: 'chat-sticky', intent: 'warning', toasterId }
            );
            stickyRef.current = id || 'chat-sticky';
            pauseToast(stickyRef.current);
        }
    }, [open, dispatchToast, dismissToast, pauseToast, toasterId, t, userInfo]);

    useEffect(() => {
        if (!open) return;
        if (endRef.current) endRef.current.scrollIntoView({ behavior: 'smooth' });
    }, [open, sessionId, started, activeSessionId]);

    useEffect(() => {
        const handler = () => {
            if (endRef.current) endRef.current.scrollIntoView({ behavior: 'smooth' });
        };
        window.addEventListener('chat-bubbles-updated', handler);
        return () => window.removeEventListener('chat-bubbles-updated', handler);
    }, []);

    useEffect(() => {
        if (!open || !apiBase || userInfo === null) return;
        const me = userInfo?.id || null;
        fetchSessions().then(async items => {
            setSessions(items);
            if (items.length > 0) {
                setActiveSessionId(items[0].id);
                await selectSession(items[0].id);
            } else {
                setActiveSessionId(null);
                await selectSession(null);
            }
            const peerIds = new Set();
            for (const it of items) {
                const peerId = it.senderId === me ? it.recipientId : it.senderId;
                if (peerId) peerIds.add(peerId);
            }
            const map = {};
            await Promise.all([...peerIds].map(async pid => {
                map[pid] = await fetchUserMini(pid);
            }));
            setPeers(map);
        }).catch(async () => {
            setSessions([]);
            setPeers({});
            setActiveSessionId(null);
            await selectSession(null);
        });
    }, [open, apiBase, userInfo, selectSession]);

    const resolvedSessionId = useMemo(() => {
        if (activeSessionId) return activeSessionId;
        return sessionId;
    }, [activeSessionId, sessionId]);

    const handleStart = async () => {
        if (!mathInfo?.id || !userInfo?.id) return;
        const existing = sessions.find(sx =>
            (sx.senderId === userInfo.id && sx.recipientId === mathInfo.id) ||
            (sx.senderId === mathInfo.id && sx.recipientId === userInfo.id)
        );
        if (existing) {
            setActiveSessionId(existing.id);
            await selectSession(existing.id);
            return;
        }
        const id = await ensureSession(mathInfo.id);
        if (id) {
            setActiveSessionId(id);
            await selectSession(id);
        }
        if (onStart) onStart();
    };

    useEffect(() => {
        if (!bubblesRef.current) return;
        bubblesRef.current.api = apiBase ?? "";
        bubblesRef.current.sessionId = resolvedSessionId ?? "";
        bubblesRef.current.authToken = token ?? "";
        bubblesRef.current.senderUserId = userInfo?.id ?? "";
    }, [apiBase, resolvedSessionId, token, userInfo?.id]);

    const showStart = !isOwner && sessions.length === 0;

    if (!open) return null;

    return (
        <div className={s.container}>
            <div className={s.sidebar}>
                <div className={s.userCard}>
                    {sessions.map(it => {
                        const me = userInfo?.id || null;
                        const peerId = it.senderId === me ? it.recipientId : it.senderId;
                        const peer = (peerId && peers[peerId]) ? peers[peerId] : { name: 'Usuário', email: '', picture: '' };
                        return (
                            <div
                                key={it.id}
                                className={s.listItem}
                                role="button"
                                tabIndex={0}
                                onClick={async () => {
                                    setActiveSessionId(it.id);
                                    await selectSession(it.id);
                                    setTimeout(() => { if (endRef.current) endRef.current.scrollIntoView({ behavior: 'smooth' }); }, 0);
                                }}
                                onKeyDown={async (e) => {
                                    if (e.key === 'Enter' || e.key === ' ') {
                                        setActiveSessionId(it.id);
                                        await selectSession(it.id);
                                        setTimeout(() => { if (endRef.current) endRef.current.scrollIntoView({ behavior: 'smooth' }); }, 0);
                                    }
                                }}
                            >
                                <Button appearance="transparent" className={s.listItemBtn}>
                                    <Persona
                                        className={s.persona}
                                        name={peer.name}
                                        secondaryText={peer.email}
                                        avatar={{ image: { src: peer.picture } }}
                                        presence={{ status: online ? 'available' : 'offline' }}
                                    />
                                </Button>
                            </div>
                        );
                    })}
                </div>
            </div>

            <div className={s.chatArea}>
                <div className={s.topBar}>
                    <HeaderWithStatus apiBase={apiBase} />
                    <Button appearance="subtle" icon={<Dismiss24Regular />} onClick={onClose} />
                </div>

                <div className={s.messagesWrapper} ref={wrapperRef}>
                    <Toaster inline position="bottom" toasterId={toasterId} limit={1} />
                    {showStart ? (
                        <div className={s.emptyState}>
                            <StartChatButton onStarted={handleStart} />
                        </div>
                    ) : (
                        <>
                                {resolvedSessionId && userInfo?.id && (
                                <chat-bubbles
                                    ref={bubblesRef}
                                />
                            )}
                            <div ref={endRef} />
                        </>
                    )}
                </div>

                {!isOwner && !showStart && (
                    <div className={s.inputBar}>
                        <ChatInput
                            value={input}
                            onChange={setInput}
                            onSend={async (msg) => {
                                onSend(msg);
                                setInput('');
                                setTimeout(() => { if (endRef.current) endRef.current.scrollIntoView({ behavior: 'smooth' }); }, 0);
                            }}
                        />
                    </div>
                )}
            </div>
        </div>
    );
}
