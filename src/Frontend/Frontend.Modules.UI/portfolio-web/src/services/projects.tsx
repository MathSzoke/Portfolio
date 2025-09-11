// services/projects.ts
export type ProjectSource = 'aspire' | 'external';

export type ProjectVm = {
    slug: string;
    name: string;
    summary: string;
    technologies: string[];
    image?: string | null;
    repoUrl?: string | null;
    liveUrl?: string | null;
    source: ProjectSource;
};

type ApiProject = {
    slug: string;
    name: string;
    summary: string;
    technologies: string[];
    repoUrl?: string | null;
    liveUrl?: string | null;
    icon?: string | null;
};

type ExternalProject = {
    slug: string;
    name: string;
    summary: string;
    technologies: string[];
    repoUrl?: string | null;
    liveUrl?: string | null;
    image?: string | null;
};

type FetchOpts = { signal?: AbortSignal };

function getApiBase(): string | undefined {
    const env = import.meta.env as unknown as Record<string, string | undefined>;
    return env.VITE_PORTFOLIO_API ?? env.VITE_API_BASE_URL;
}

async function getProjectsFromApi(opts?: FetchOpts): Promise<ProjectVm[]> {
    const api = getApiBase();
    if (!api) return [];
    const res = await fetch(`${api}/projects`, { signal: opts?.signal });
    if (!res.ok) return [];
    const data = (await res.json()) as ApiProject[];
    return data.map<ProjectVm>(p => ({
        slug: p.slug,
        name: p.name,
        summary: p.summary,
        technologies: p.technologies ?? [],
        image: p.icon ?? null,
        repoUrl: p.repoUrl ?? null,
        liveUrl: p.liveUrl ?? null,
        source: 'aspire',
    }));
}

async function getExternalProjects(opts?: FetchOpts): Promise<ProjectVm[]> {
    const base = (import.meta.env.BASE_URL ?? '/') as string;
    const url = new URL('external-projects.json', base).toString();
    const res = await fetch(url, { signal: opts?.signal });
    if (!res.ok) return [];
    const data = (await res.json()) as ExternalProject[];
    return data.map<ProjectVm>(p => ({
        slug: p.slug,
        name: p.name,
        summary: p.summary,
        technologies: p.technologies ?? [],
        image: p.image ?? null,
        repoUrl: p.repoUrl ?? null,
        liveUrl: p.liveUrl ?? null,
        source: 'external',
    }));
}

function mergeAndDedupe(aspire: ProjectVm[], external: ProjectVm[]): ProjectVm[] {
    const map = new Map<string, ProjectVm>();
    for (const p of external) map.set(p.slug, p);
    for (const p of aspire) map.set(p.slug, p);
    return [...map.values()].sort((a, b) => a.name.localeCompare(b.name));
}

let _cache: { when: number; data: ProjectVm[] } | null = null;
const CACHE_MS = 30_000;

export async function getAggregatedProjects(opts?: FetchOpts): Promise<ProjectVm[]> {
    const now = Date.now();
    if (_cache && now - _cache.when < CACHE_MS) return _cache.data;
    const [fromApi, fromExternal] = await Promise.allSettled([
        getProjectsFromApi(opts),
        getExternalProjects(opts),
    ]);
    const apiList = fromApi.status === 'fulfilled' ? fromApi.value : [];
    const extList = fromExternal.status === 'fulfilled' ? fromExternal.value : [];
    const merged = mergeAndDedupe(apiList, extList);
    _cache = { when: now, data: merged };
    return merged;
}

export type ReorderItem = { slug: string; order: number };

export async function reorderProjects(items: ReorderItem[], opts?: FetchOpts): Promise<void> {
    const api = getApiBase();
    if (!api) throw new Error('API base URL não configurada');
    const { getAuthHeaders } = await import('./auth.tsx');
    const res = await fetch(`${api}/projects/reorder`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            ...getAuthHeaders(),
        },
        body: JSON.stringify({ items }),
        signal: opts?.signal,
    });
    if (!res.ok) {
        const text = await res.text().catch(() => '');
        throw new Error(text || `HTTP ${res.status}`);
    }
    _cache = null;
}
