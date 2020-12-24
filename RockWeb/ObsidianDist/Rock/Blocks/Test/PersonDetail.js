var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
define(["require", "exports", "../../Util/bus.js", "../../Templates/PaneledBlockTemplate.js", "../../Elements/RockButton.js", "../../Elements/TextBox.js", "../../Vendor/Vue/vue.js", "../../Store/index.js"], function (require, exports, bus_js_1, PaneledBlockTemplate_js_1, RockButton_js_1, TextBox_js_1, vue_js_1, index_js_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = vue_js_1.defineComponent({
        name: 'Test.PersonDetail',
        components: {
            PaneledBlockTemplate: PaneledBlockTemplate_js_1.default,
            RockButton: RockButton_js_1.default,
            TextBox: TextBox_js_1.default
        },
        data: function () {
            var person = {
                FirstName: 'Ted',
                LastName: 'Decker'
            };
            return {
                person: person,
                personForEditing: __assign({}, person),
                isEditMode: false,
                messageToPublish: '',
                receivedMessage: ''
            };
        },
        methods: {
            setAreSecondaryBlocksShown: function (isVisible) {
                index_js_1.default.commit('setAreSecondaryBlocksShown', { areSecondaryBlocksShown: isVisible });
            },
            setIsEditMode: function (isEditMode) {
                this.isEditMode = isEditMode;
                this.setAreSecondaryBlocksShown(!isEditMode);
            },
            doEdit: function () {
                this.personForEditing = __assign({}, this.person);
                this.setIsEditMode(true);
            },
            doDelete: function () {
                console.log('delete here');
            },
            doCancel: function () {
                this.personForEditing = __assign({}, this.person);
                this.setIsEditMode(false);
            },
            doSave: function () {
                this.person = __assign({}, this.personForEditing);
                this.setIsEditMode(false);
            },
            doPublish: function () {
                bus_js_1.default.publish('PersonDetail:Message', this.messageToPublish);
                this.messageToPublish = '';
            },
            receiveMessage: function (message) {
                this.receivedMessage = message;
            }
        },
        computed: {
            blockTitle: function () {
                return this.person.FirstName + " " + this.person.LastName;
            }
        },
        created: function () {
            bus_js_1.default.subscribe('PersonSecondary:Message', this.receiveMessage);
        },
        template: "<PaneledBlockTemplate>\n    <template v-slot:title>\n        <i class=\"fa fa-flask\"></i>\n        Detail Block: {{blockTitle}}\n    </template>\n    <template v-slot:default>\n        <template v-if=\"isEditMode\">\n            <div class=\"row\">\n                <div class=\"col-sm-6 col-lg-4\">\n                    <TextBox label=\"First Name\" v-model=\"personForEditing.FirstName\" />\n                    <TextBox label=\"Last Name\" v-model=\"personForEditing.LastName\" />\n                </div>\n            </div>\n        </template>\n        <template v-else>\n            <div class=\"row\">\n                <div class=\"col-sm-6\">\n                    <dl>\n                        <dt>First Name</dt>\n                        <dd>{{person.FirstName}}</dd>\n                        <dt>Last Name</dt>\n                        <dd>{{person.LastName}}</dd>\n                    </dl>\n                </div>\n                <div class=\"col-sm-6\">\n                    <div class=\"well\">\n                        <TextBox label=\"Message\" v-model=\"messageToPublish\" />\n                        <RockButton class=\"btn-primary btn-sm\" @click=\"doPublish\">Publish</RockButton>\n                    </div>\n                    <p>\n                        <strong>Secondary block says:</strong>\n                        {{receivedMessage}}\n                    </p>\n                </div>\n            </div>\n        </template>\n        <div class=\"actions\">\n            <template v-if=\"isEditMode\">\n                <RockButton class=\"btn-primary\" @click=\"doSave\">Save</RockButton>\n                <RockButton class=\"btn-link\" @click=\"doCancel\">Cancel</RockButton>\n            </template>\n            <template v-else>\n                <RockButton class=\"btn-primary\" @click=\"doEdit\">Edit</RockButton>\n                <RockButton class=\"btn-link\" @click=\"doDelete\">Delete</RockButton>\n            </template>\n        </div>\n    </template>\n</PaneledBlockTemplate>"
    });
});
//# sourceMappingURL=PersonDetail.js.map