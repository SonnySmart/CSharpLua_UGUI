-- Generated By protoc-gen-lua Do not Edit
local protobuf = require "protobuf/protobuf"
module('PersonProto_pb')


local localTable = {}
PERSONPROTO = protobuf.Descriptor()
PERSONPROTO_HEADER_ = protobuf.Descriptor()
localTable.PERSONPROTO_HEADER__CMD_FIELD = protobuf.FieldDescriptor()
localTable.PERSONPROTO_HEADER__SEQ_FIELD = protobuf.FieldDescriptor()
PERSONPROTO_HEADER2_ = protobuf.Descriptor()
localTable.PERSONPROTO_HEADER2__CMD_FIELD = protobuf.FieldDescriptor()
localTable.PERSONPROTO_HEADER2__SEQ_FIELD = protobuf.FieldDescriptor()
localTable.PERSONPROTO_HEADER2__SS_FIELD = protobuf.FieldDescriptor()
localTable.PERSONPROTO_HEADER_FIELD = protobuf.FieldDescriptor()
localTable.PERSONPROTO_ID_FIELD = protobuf.FieldDescriptor()
localTable.PERSONPROTO_NAME_FIELD = protobuf.FieldDescriptor()
localTable.PERSONPROTO_AGE_FIELD = protobuf.FieldDescriptor()
localTable.PERSONPROTO_EMAIL_FIELD = protobuf.FieldDescriptor()
localTable.PERSONPROTO_ARRAYS_FIELD = protobuf.FieldDescriptor()
localTable.PERSONPROTO_ARRAY2S_FIELD = protobuf.FieldDescriptor()
localTable.PERSONPROTO_HEADER2_FIELD = protobuf.FieldDescriptor()


localTable.PERSONPROTO_HEADER__CMD_FIELD.name = "cmd"
localTable.PERSONPROTO_HEADER__CMD_FIELD.full_name = "Protobuf.PersonProto.Header_.cmd"
localTable.PERSONPROTO_HEADER__CMD_FIELD.number = 1
localTable.PERSONPROTO_HEADER__CMD_FIELD.index = 0
localTable.PERSONPROTO_HEADER__CMD_FIELD.label = 1
localTable.PERSONPROTO_HEADER__CMD_FIELD.has_default_value = false
localTable.PERSONPROTO_HEADER__CMD_FIELD.default_value = 0
localTable.PERSONPROTO_HEADER__CMD_FIELD.type = 5
localTable.PERSONPROTO_HEADER__CMD_FIELD.cpp_type = 1

localTable.PERSONPROTO_HEADER__SEQ_FIELD.name = "seq"
localTable.PERSONPROTO_HEADER__SEQ_FIELD.full_name = "Protobuf.PersonProto.Header_.seq"
localTable.PERSONPROTO_HEADER__SEQ_FIELD.number = 2
localTable.PERSONPROTO_HEADER__SEQ_FIELD.index = 1
localTable.PERSONPROTO_HEADER__SEQ_FIELD.label = 1
localTable.PERSONPROTO_HEADER__SEQ_FIELD.has_default_value = false
localTable.PERSONPROTO_HEADER__SEQ_FIELD.default_value = 0
localTable.PERSONPROTO_HEADER__SEQ_FIELD.type = 5
localTable.PERSONPROTO_HEADER__SEQ_FIELD.cpp_type = 1

PERSONPROTO_HEADER_.name = "Header_"
PERSONPROTO_HEADER_.full_name = "Protobuf.PersonProto.Header_"
PERSONPROTO_HEADER_.nested_types = {}
PERSONPROTO_HEADER_.enum_types = {}
PERSONPROTO_HEADER_.fields = {localTable.PERSONPROTO_HEADER__CMD_FIELD, localTable.PERSONPROTO_HEADER__SEQ_FIELD}
PERSONPROTO_HEADER_.is_extendable = false
PERSONPROTO_HEADER_.extensions = {}
PERSONPROTO_HEADER_.containing_type = PERSONPROTO
localTable.PERSONPROTO_HEADER2__CMD_FIELD.name = "cmd"
localTable.PERSONPROTO_HEADER2__CMD_FIELD.full_name = "Protobuf.PersonProto.Header2_.cmd"
localTable.PERSONPROTO_HEADER2__CMD_FIELD.number = 1
localTable.PERSONPROTO_HEADER2__CMD_FIELD.index = 0
localTable.PERSONPROTO_HEADER2__CMD_FIELD.label = 1
localTable.PERSONPROTO_HEADER2__CMD_FIELD.has_default_value = false
localTable.PERSONPROTO_HEADER2__CMD_FIELD.default_value = 0
localTable.PERSONPROTO_HEADER2__CMD_FIELD.type = 5
localTable.PERSONPROTO_HEADER2__CMD_FIELD.cpp_type = 1

localTable.PERSONPROTO_HEADER2__SEQ_FIELD.name = "seq"
localTable.PERSONPROTO_HEADER2__SEQ_FIELD.full_name = "Protobuf.PersonProto.Header2_.seq"
localTable.PERSONPROTO_HEADER2__SEQ_FIELD.number = 2
localTable.PERSONPROTO_HEADER2__SEQ_FIELD.index = 1
localTable.PERSONPROTO_HEADER2__SEQ_FIELD.label = 1
localTable.PERSONPROTO_HEADER2__SEQ_FIELD.has_default_value = false
localTable.PERSONPROTO_HEADER2__SEQ_FIELD.default_value = 0
localTable.PERSONPROTO_HEADER2__SEQ_FIELD.type = 5
localTable.PERSONPROTO_HEADER2__SEQ_FIELD.cpp_type = 1

localTable.PERSONPROTO_HEADER2__SS_FIELD.name = "ss"
localTable.PERSONPROTO_HEADER2__SS_FIELD.full_name = "Protobuf.PersonProto.Header2_.ss"
localTable.PERSONPROTO_HEADER2__SS_FIELD.number = 3
localTable.PERSONPROTO_HEADER2__SS_FIELD.index = 2
localTable.PERSONPROTO_HEADER2__SS_FIELD.label = 1
localTable.PERSONPROTO_HEADER2__SS_FIELD.has_default_value = false
localTable.PERSONPROTO_HEADER2__SS_FIELD.default_value = ""
localTable.PERSONPROTO_HEADER2__SS_FIELD.type = 9
localTable.PERSONPROTO_HEADER2__SS_FIELD.cpp_type = 9

PERSONPROTO_HEADER2_.name = "Header2_"
PERSONPROTO_HEADER2_.full_name = "Protobuf.PersonProto.Header2_"
PERSONPROTO_HEADER2_.nested_types = {}
PERSONPROTO_HEADER2_.enum_types = {}
PERSONPROTO_HEADER2_.fields = {localTable.PERSONPROTO_HEADER2__CMD_FIELD, localTable.PERSONPROTO_HEADER2__SEQ_FIELD, localTable.PERSONPROTO_HEADER2__SS_FIELD}
PERSONPROTO_HEADER2_.is_extendable = false
PERSONPROTO_HEADER2_.extensions = {}
PERSONPROTO_HEADER2_.containing_type = PERSONPROTO
localTable.PERSONPROTO_HEADER_FIELD.name = "header"
localTable.PERSONPROTO_HEADER_FIELD.full_name = "Protobuf.PersonProto.header"
localTable.PERSONPROTO_HEADER_FIELD.number = 1
localTable.PERSONPROTO_HEADER_FIELD.index = 0
localTable.PERSONPROTO_HEADER_FIELD.label = 1
localTable.PERSONPROTO_HEADER_FIELD.has_default_value = false
localTable.PERSONPROTO_HEADER_FIELD.default_value = nil
localTable.PERSONPROTO_HEADER_FIELD.message_type = PERSONPROTO_HEADER_
localTable.PERSONPROTO_HEADER_FIELD.type = 11
localTable.PERSONPROTO_HEADER_FIELD.cpp_type = 10

localTable.PERSONPROTO_ID_FIELD.name = "id"
localTable.PERSONPROTO_ID_FIELD.full_name = "Protobuf.PersonProto.id"
localTable.PERSONPROTO_ID_FIELD.number = 2
localTable.PERSONPROTO_ID_FIELD.index = 1
localTable.PERSONPROTO_ID_FIELD.label = 1
localTable.PERSONPROTO_ID_FIELD.has_default_value = false
localTable.PERSONPROTO_ID_FIELD.default_value = 0
localTable.PERSONPROTO_ID_FIELD.type = 5
localTable.PERSONPROTO_ID_FIELD.cpp_type = 1

localTable.PERSONPROTO_NAME_FIELD.name = "name"
localTable.PERSONPROTO_NAME_FIELD.full_name = "Protobuf.PersonProto.name"
localTable.PERSONPROTO_NAME_FIELD.number = 3
localTable.PERSONPROTO_NAME_FIELD.index = 2
localTable.PERSONPROTO_NAME_FIELD.label = 1
localTable.PERSONPROTO_NAME_FIELD.has_default_value = false
localTable.PERSONPROTO_NAME_FIELD.default_value = ""
localTable.PERSONPROTO_NAME_FIELD.type = 9
localTable.PERSONPROTO_NAME_FIELD.cpp_type = 9

localTable.PERSONPROTO_AGE_FIELD.name = "age"
localTable.PERSONPROTO_AGE_FIELD.full_name = "Protobuf.PersonProto.age"
localTable.PERSONPROTO_AGE_FIELD.number = 4
localTable.PERSONPROTO_AGE_FIELD.index = 3
localTable.PERSONPROTO_AGE_FIELD.label = 1
localTable.PERSONPROTO_AGE_FIELD.has_default_value = false
localTable.PERSONPROTO_AGE_FIELD.default_value = 0
localTable.PERSONPROTO_AGE_FIELD.type = 5
localTable.PERSONPROTO_AGE_FIELD.cpp_type = 1

localTable.PERSONPROTO_EMAIL_FIELD.name = "email"
localTable.PERSONPROTO_EMAIL_FIELD.full_name = "Protobuf.PersonProto.email"
localTable.PERSONPROTO_EMAIL_FIELD.number = 5
localTable.PERSONPROTO_EMAIL_FIELD.index = 4
localTable.PERSONPROTO_EMAIL_FIELD.label = 1
localTable.PERSONPROTO_EMAIL_FIELD.has_default_value = false
localTable.PERSONPROTO_EMAIL_FIELD.default_value = ""
localTable.PERSONPROTO_EMAIL_FIELD.type = 9
localTable.PERSONPROTO_EMAIL_FIELD.cpp_type = 9

localTable.PERSONPROTO_ARRAYS_FIELD.name = "arrays"
localTable.PERSONPROTO_ARRAYS_FIELD.full_name = "Protobuf.PersonProto.arrays"
localTable.PERSONPROTO_ARRAYS_FIELD.number = 6
localTable.PERSONPROTO_ARRAYS_FIELD.index = 5
localTable.PERSONPROTO_ARRAYS_FIELD.label = 3
localTable.PERSONPROTO_ARRAYS_FIELD.has_default_value = false
localTable.PERSONPROTO_ARRAYS_FIELD.default_value = {}
localTable.PERSONPROTO_ARRAYS_FIELD.type = 5
localTable.PERSONPROTO_ARRAYS_FIELD.cpp_type = 1

localTable.PERSONPROTO_ARRAY2S_FIELD.name = "array2s"
localTable.PERSONPROTO_ARRAY2S_FIELD.full_name = "Protobuf.PersonProto.array2s"
localTable.PERSONPROTO_ARRAY2S_FIELD.number = 7
localTable.PERSONPROTO_ARRAY2S_FIELD.index = 6
localTable.PERSONPROTO_ARRAY2S_FIELD.label = 3
localTable.PERSONPROTO_ARRAY2S_FIELD.has_default_value = false
localTable.PERSONPROTO_ARRAY2S_FIELD.default_value = {}
localTable.PERSONPROTO_ARRAY2S_FIELD.type = 9
localTable.PERSONPROTO_ARRAY2S_FIELD.cpp_type = 9

localTable.PERSONPROTO_HEADER2_FIELD.name = "header2"
localTable.PERSONPROTO_HEADER2_FIELD.full_name = "Protobuf.PersonProto.header2"
localTable.PERSONPROTO_HEADER2_FIELD.number = 8
localTable.PERSONPROTO_HEADER2_FIELD.index = 7
localTable.PERSONPROTO_HEADER2_FIELD.label = 1
localTable.PERSONPROTO_HEADER2_FIELD.has_default_value = false
localTable.PERSONPROTO_HEADER2_FIELD.default_value = nil
localTable.PERSONPROTO_HEADER2_FIELD.message_type = PERSONPROTO_HEADER2_
localTable.PERSONPROTO_HEADER2_FIELD.type = 11
localTable.PERSONPROTO_HEADER2_FIELD.cpp_type = 10

PERSONPROTO.name = "PersonProto"
PERSONPROTO.full_name = "Protobuf.PersonProto"
PERSONPROTO.nested_types = {PERSONPROTO_HEADER_, PERSONPROTO_HEADER2_}
PERSONPROTO.enum_types = {}
PERSONPROTO.fields = {localTable.PERSONPROTO_HEADER_FIELD, localTable.PERSONPROTO_ID_FIELD, localTable.PERSONPROTO_NAME_FIELD, localTable.PERSONPROTO_AGE_FIELD, localTable.PERSONPROTO_EMAIL_FIELD, localTable.PERSONPROTO_ARRAYS_FIELD, localTable.PERSONPROTO_ARRAY2S_FIELD, localTable.PERSONPROTO_HEADER2_FIELD}
PERSONPROTO.is_extendable = false
PERSONPROTO.extensions = {}

PersonProto = protobuf.Message(PERSONPROTO)
PersonProto.Header2_ = protobuf.Message(PERSONPROTO_HEADER2_)
PersonProto.Header_ = protobuf.Message(PERSONPROTO_HEADER_)

