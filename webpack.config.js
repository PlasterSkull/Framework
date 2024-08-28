const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const FixStyleOnlyEntriesPlugin = require("webpack-fix-style-only-entries");
const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");

module.exports = (env, args) => {

    const isDevelopment = args.mode === 'development';

    const config = {
        mode: args.mode,
        entry: {
            plasterSkullFrameworkBlazor: {
                import: "./src/Blazor/PlasterSkull.Framework.Blazor/wwwroot/index.ts",
                filename: "src/Blazor/PlasterSkull.Framework.Blazor/wwwroot/index.js",
                dependOn: 'plasterSkullFrameworkBlazorInfrastructure',
            },
            plasterSkullFrameworkBlazorScss: "./src/Blazor/PlasterSkull.Framework.Blazor/wwwroot/index.scss",
            plasterSkullFrameworkBlazorInfrastructure: {
                import: "./src/Blazor/PlasterSkull.Framework.Blazor.Infrastructure/wwwroot/index.ts",
                filename: "src/Blazor/PlasterSkull.Framework.Blazor.Infrastructure/wwwroot/index.js",
            },
            plasterSkullFrameworkBlazorInfrastructureScss: "./src/Blazor/PlasterSkull.Framework.Blazor.Infrastructure/wwwroot/index.scss",
            plasterSkullFrameworkBlazorDemo: "./samples/PlasterSkull.Framework.Blazor.Demo/wwwroot/css/app.scss"
        },

        output: {
            path: __dirname,
        },

        optimization: {
            runtimeChunk: 'single',
            splitChunks: {
                chunks: 'all',
            },
            minimize: !isDevelopment,
            minimizer: [
                new CssMinimizerPlugin(),
            ],
        },

        plugins: [
            new MiniCssExtractPlugin(),
            new FixStyleOnlyEntriesPlugin({ extensions: ['scss'] }),
        ],

        module: {
            rules: [
                {
                    test: /\.(ts|tsx)$/,
                    loader: 'ts-loader',
                    include: [path.resolve(__dirname, 'src')],
                    exclude: [/node_modules/]
                },
                {
                    test: /\.scss$/,
                    exclude: /node_modules/,
                    type: 'asset/resource',
                    generator: {
                        filename: '[path]/[name].css',
                    },
                    use: [
                        'sass-loader'
                    ]
                },
            ]
        },
    };
    return config;
};