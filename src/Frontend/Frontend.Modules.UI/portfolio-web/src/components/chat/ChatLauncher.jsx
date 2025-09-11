import { useEffect, useRef, useState } from 'react';
import {
    Button,
    Input,
    Persona,
    makeStyles,
    tokens,
    Avatar
} from '@fluentui/react-components';
import {
    ArrowUpFilled,
    ArrowDownFilled,
    SendFilled,
    Dismiss24Regular,
    SearchRegular
} from '@fluentui/react-icons';
import { useAuth } from '../../services/auth';
import { useTranslation } from 'react-i18next';
import {ChatInput} from "./ChatInput.jsx";

const useStyles = makeStyles({
    floatingButton: {
        position: 'fixed',
        right: '24px',
        bottom: '24px',
        zIndex: 9999
    },
    container: {
        position: 'fixed',
        right: '24px',
        bottom: '88px',
        zIndex: 9999,
        width: '800px',
        height: '600px',
        display: 'flex',
        background: tokens.colorNeutralBackground1,
        border: `1px solid ${tokens.colorNeutralStroke2}`,
        borderRadius: '18px',
        boxShadow: '0 4px 32px #0002'
    },
    sidebar: {
        width: '340px',
        background: tokens.colorNeutralBackground2,
        borderTopLeftRadius: '18px',
        borderBottomLeftRadius: '18px',
        borderRight: `1px solid ${tokens.colorNeutralStroke2}`,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        padding: '28px 0 0 0'
    },
    userCard: {
        width: '85%',
        borderRadius: '18px',
        background: tokens.colorNeutralBackground3,
        boxShadow: tokens.shadow2,
        display: 'flex',
        flexDirection: 'column',
        marginBottom: '30px'
    },
    persona: {
        margin: '14px'
    },
    userEmail: {
        fontSize: '13px',
        color: '#444',
        marginTop: '2px',
        marginBottom: '2px',
        wordBreak: 'break-all'
    },
    chatArea: {
        flex: 1,
        display: 'flex',
        flexDirection: 'column',
        background: tokens.colorNeutralBackground1Hover,
        borderTopRightRadius: '18px',
        borderBottomRightRadius: '18px'
    },
    topBar: {
        height: '52px',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'flex-end',
        padding: '0 10px',
        borderBottom: `1px solid ${tokens.colorNeutralStroke2}`
    },
    messagesWrapper: {
        flex: 1,
        overflowY: 'auto',
        padding: '1em',
        background: tokens.colorNeutralBackground1Hover,
        display: 'flex',
        flexDirection: 'column'
    },
    emptyState: {
        flex: 1,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center'
    },
    inputBar: {
        background: tokens.colorNeutralBackground1,
        borderRadius: '26px',
        marginRight: '12px',
        marginLeft: '12px',
        marginBottom: '12px'
    },
    form: {
        display: 'flex',
        gap: '12px'
    },
    chatBubbleRow: {
        display: 'flex',
        alignItems: 'flex-end',
        gap: '10px',
        margin: '16px 0'
    },
    chatBubble: {
        position: 'relative',
        maxWidth: '300px',
        padding: '15px',
        borderRadius: '16px',
        fontSize: '12px'
    },
    chatBubbleFromMe: {
        background: tokens.colorBrandForeground1,
        color: tokens.colorNeutralBackground1,
        borderBottomRightRadius: '6px',
        borderBottomLeftRadius: '18px',
        marginLeft: 'auto',
        overflowWrap: 'break-word',
        marginRight: '1em',
        '&::after': {
            content: '""',
            position: 'absolute',
            right: '-10px',
            bottom: '6px',
            width: 0,
            height: 0,
            borderTop: '10px solid transparent',
            borderBottom: '10px solid transparent',
            borderLeft: `14px solid ${tokens.colorBrandForeground1}`,
        }
    },
    chatBubbleFromOther: {
        background: '#9399a0',
        color: '#fff',
        borderBottomRightRadius: '18px',
        borderBottomLeftRadius: '6px',
        marginRight: 'auto',
        '&::after': {
            content: '""',
            position: 'absolute',
            left: '-14px',
            bottom: '2px',
            width: 0,
            height: 0,
            borderTop: '10px solid transparent',
            borderBottom: '10px solid transparent',
            borderRight: '14px solid #9399a0',
        }
    }
});

function ChatBubble({ fromMe, children }) {
    const s = useStyles();
    const { userInfo } = useAuth();
    return (
        <div className={s.chatBubbleRow} style={{ flexDirection: fromMe ? 'row-reverse' : 'row' }}>
            <Avatar
                name={userInfo.name}
                image={{
                    src: userInfo.picture,
                }}
            />
            <div className={`${s.chatBubble} ${fromMe ? s.chatBubbleFromMe : s.chatBubbleFromOther}`}>
                {children}
            </div>
        </div>
    );
}

function ChatDialog({ open, onClose, messages, onSend, onStart, started }) {
    const s = useStyles();
    const [input, setInput] = useState('');
    const endRef = useRef(null);
    const { mathInfo } = useAuth();
    const { t } = useTranslation();

    useEffect(() => {
        if (open && endRef.current) endRef.current.scrollIntoView({ behavior: 'smooth' });
    }, [open, messages]);

    if (!open) return null;

    return (
        <div className={s.container}>
            <div className={s.sidebar}>
                <div className={s.userCard}>
                    <Persona
                        name={mathInfo.name}
                        secondaryText={mathInfo.email}
                        presence={{ status: 'available' }}
                        avatar={{ image: { src: mathInfo.picture } }}
                        className={s.persona}
                    />
                </div>
            </div>
            <div className={s.chatArea}>
                <div className={s.topBar}>
                    <Input
                        contentBefore={<SearchRegular />}
                        placeholder={t('chat.searchPlaceholder')}
                        style={{ borderRadius: '20px', width: '50%' }}
                    />
                    <Button appearance="subtle" aria-label={t('common.close')} icon={<Dismiss24Regular />} onClick={onClose} />
                </div>
                <div className={s.messagesWrapper}>
                    {!started ? (
                        <div className={s.emptyState}>
                            <Button size="large" appearance="primary" onClick={onStart}>
                                {t('chat.startConversation')}
                            </Button>
                        </div>
                    ) : (
                        <>
                            {messages.map((msg, i) =>
                                <ChatBubble key={i} fromMe={msg.fromMe}>{msg.text}</ChatBubble>
                            )}
                            <div ref={endRef} />
                        </>
                    )}
                </div>
                {started && (
                    <div className={s.inputBar}>
                        <ChatInput
                            value={input}
                            onChange={setInput}
                            onSend={(msg) => {
                                onSend(msg);
                                setInput('');
                            }}
                        />
                    </div>
                )}
            </div>
        </div>
    );
}

export function ChatLauncherFloatingButton() {
    const s = useStyles();
    const [open, setOpen] = useState(false);
    const [messages, setMessages] = useState([]);
    const [started, setStarted] = useState(false);

    useEffect(() => {
        const handler = () => setOpen(true);
        window.addEventListener('open-chat', handler);
        return () => window.removeEventListener('open-chat', handler);
    }, []);

    function handleSend(msg) {
        setMessages(x => [...x, { fromMe: true, text: msg }]);
    }
    function handleStart() {
        setStarted(true);
        setMessages([]);
    }

    return (
        <>
            <Button
                className={s.floatingButton}
                appearance="primary"
                onClick={() => setOpen(!open)}
                icon={open ? <ArrowDownFilled /> : <ArrowUpFilled />}
            />
            <ChatDialog
                open={open}
                onClose={() => { setOpen(false); setStarted(false); setMessages([]); }}
                messages={messages}
                onSend={handleSend}
                onStart={handleStart}
                started={started}
            />
        </>
    );
}
