import type { User } from "~/types";

export default defineNuxtPlugin(() => {
    const baseURL = 'http://localhost:5001/';

    const $host = $fetch.create({
        baseURL: baseURL,
    });

    const $authHost = async (url: string, method = 'GET', headers?: any, body?: any) => {
        let currentBody: any = { method, headers: Object.assign({ Authorization: `Bearer ${(useCookie('user').value as User | undefined)?.accessToken}` }, headers) }
        if (method !== 'GET' && typeof(body) !== 'undefined') {                    
            currentBody['body'] = body;
        }
        return await $fetch(baseURL + url, currentBody)
            .catch(async e => {
                if (e.status === 401) {
                    let user = useCookie('user').value as User | undefined;

                    if (typeof(user) !== 'undefined') {
                        const tokenModel = {
                            accessToken: user.accessToken,
                            refrechToken: user.refrechToken
                        }

                        let nextTokenModel: { accessToken: string, refrechToken: string, time: string } = 
                            await $fetch(`${baseURL}api/account/refrech`, { method: 'POST', body: tokenModel });

                        useCookie('user', { expires: new Date(nextTokenModel.time) }).value = JSON.stringify({
                            ...user,
                            accessToken: nextTokenModel.accessToken,
                            refrechToken: nextTokenModel.refrechToken,
                            time: nextTokenModel.time,
                        });

                        currentBody = { ...currentBody, headers: Object.assign({ Authorization: `Bearer ${nextTokenModel.accessToken}` }, headers) };                        
                        return await $fetch(baseURL + url, currentBody).catch(_ => undefined);
                    }
                    return undefined;
                }
            });
    }
    return {
        provide: {
            host: $host,
            authHost: $authHost,
        }
    }
})
