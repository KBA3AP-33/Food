export default defineNuxtPlugin(() => {
    const $throttle = (func: Function, ms: number) => {
        let locked = false;
        return function(this: any) {
            if (locked) return;
            locked = true;

            setTimeout(() => {
                func.apply(this, arguments);
                locked = false;
            }, ms);
        }
    }

    return {
        provide: {
            throttle: $throttle,
        }
    }
});
