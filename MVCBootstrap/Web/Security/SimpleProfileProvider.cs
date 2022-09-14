//using System;

//namespace MVCBootstrap.Web.Security {

//    public class SimpleEntityFrameworkProfileProvider : ProfileProvider {

//        protected IContext UnitOfWork { get; private set; }

//        protected IRepository<Profile> ProfileRepository { get; private set; }

//        public override void Initialize(String name, NameValueCollection config) {
//            base.Initialize(name, config);

//            this.UnitOfWork = DependencyResolver.Current.GetService<IContext>();
//            this.ProfileRepository = DependencyResolver.Current.GetService<IRepository<Profile>>();
//        }

//        public override Int32 DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate) {
//            throw new NotImplementedException();
//        }

//        public override Int32 DeleteProfiles(String[] usernames) {
//            throw new NotImplementedException();
//        }

//        public override Int32 DeleteProfiles(ProfileInfoCollection profiles) {
//            throw new NotImplementedException();
//        }

//        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, String usernameToMatch, DateTime userInactiveSinceDate, Int32 pageIndex, Int32 pageSize, out Int32 totalRecords) {
//            throw new NotImplementedException();
//        }

//        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, String usernameToMatch, Int32 pageIndex, Int32 pageSize, out Int32 totalRecords) {
//            throw new NotImplementedException();
//        }

//        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, Int32 pageIndex, Int32 pageSize, out Int32 totalRecords) {
//            throw new NotImplementedException();
//        }

//        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, Int32 pageIndex, Int32 pageSize, out Int32 totalRecords) {
//            throw new NotImplementedException();
//        }

//        public override Int32 GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate) {
//            throw new NotImplementedException();
//        }

//        public override String ApplicationName { get; set; }

//        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties) {
//            SettingsPropertyValueCollection settingsPropertyValueCollections = new SettingsPropertyValueCollection();
//            if (properties.Count < 1) {
//                return settingsPropertyValueCollections;
//            }

//            String item = (String)context["UserName"];
//            foreach (SettingsProperty property in properties) {
//                if (property.SerializeAs == SettingsSerializeAs.ProviderSpecific) {
//                    if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(String)) {
//                        property.SerializeAs = SettingsSerializeAs.String;
//                        continue;
//                    }
//                    property.SerializeAs = SettingsSerializeAs.Xml;
//                }
//                settingsPropertyValueCollections.Add(new SettingsPropertyValue(property));
//            }
//            if (!String.IsNullOrEmpty(item)) {
//                Profile profile = this.ProfileRepository.ReadOne(new ProfileSpecifications.SpecificUsername(item));
//                if (profile == null) {
//                    profile = new Profile { Username = item, LastUpdated = DateTime.UtcNow };
//                    this.ProfileRepository.Create(profile);
//                    this.UnitOfWork.SaveChanges();

//                    profile = this.ProfileRepository.ReadOne(new ProfileSpecifications.SpecificUsername(item));
//                }

//                foreach (SettingsProperty property in properties) {
//                    if (profile.Settings != null && profile.Settings.Where(s => s.Name == property.Name).Any()) {
//                        ProfileSetting setting = profile.Settings.Where(s => s.Name == property.Name).First();
//                        SettingsPropertyValue value = settingsPropertyValueCollections[property.Name];
//                        if (value != null) {
//                            if (value.Property.SerializeAs == SettingsSerializeAs.Binary) {
//                                value.SerializedValue = setting.BinaryValue;
//                            }
//                            else {
//                                value.SerializedValue = setting.StringValue;
//                            }
//                        }
//                    }
//                }
//            }

//            return settingsPropertyValueCollections;
//        }

//        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection properties) {
//            String item = (String)context["UserName"];
//            Boolean userIsAuthenticated = (Boolean)context["IsAuthenticated"];
//            if (item == null || item.Length < 1 || properties.Count < 1) {
//                return;
//            }

//            Boolean flag = false;
//            foreach (SettingsPropertyValue property in properties) {
//                if (property.IsDirty && (userIsAuthenticated || (Boolean)property.Property.Attributes["AllowAnonymous"])) {
//                    flag = true;
//                    break;
//                }
//            }
//            if (flag) {
//                foreach (SettingsPropertyValue settingsPropertyValue in properties) {

//                    if ((userIsAuthenticated || (Boolean)settingsPropertyValue.Property.Attributes["AllowAnonymous"]) && (settingsPropertyValue.IsDirty || !settingsPropertyValue.UsingDefaultValue)) {




//                    }



//                }
//            }
//        }
//    }
//}