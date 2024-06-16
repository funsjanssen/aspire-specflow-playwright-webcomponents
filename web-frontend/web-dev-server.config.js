/**
 * @license
 * Copyright 2021 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */

import {legacyPlugin} from '@web/dev-server-legacy';
import proxy from 'koa-proxies';

const mode = process.env.MODE || 'dev';
if (!['dev', 'prod'].includes(mode)) {
    throw new Error(`MODE must be "dev" or "prod", was "${mode}"`);
}

export default {
    nodeResolve: {exportConditions: mode === 'dev' ? ['development'] : []},
    preserveSymlinks: true,
    port: process.env.PORT,
    appIndex: 'index.html',
    middleware: [
        proxy('/api',
            {
                target: process.env.services__apiservice__http__0,
                changeOrigin: true
            })
    ],
    plugins: [
        legacyPlugin({
            polyfills: {
                // Manually imported in index.html file
                webcomponents: false
            },
        }),
    ],
};
