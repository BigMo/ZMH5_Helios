using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.BSP.MDL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct studiohdr_t
    {
        public int id;
        public int version;

        public int checksum;       // this has to be the same in the phy and vtx files to load!

        //const char* pszName( void ) const { if (studiohdr2index && pStudioHdr2()->pszName()) return pStudioHdr2()->pszName(); else return name; }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public int length;


        public Vector3 eyeposition; // ideal eye position

        public Vector3 illumposition;   // illumination center

        public Vector3 hull_min;        // ideal movement hull size
        public Vector3 hull_max;

        public Vector3 view_bbmin;      // clipping bounding box
        public Vector3 view_bbmax;

        public int flags;

        public int numbones;           // bones
        public int boneindex;
        //inline mstudiobone_t * pBone(public int i) const { Assert(i >= 0 && i<numbones); return (mstudiobone_t*) (((byte*)this) + boneindex) + i; };
        //public int RemapSeqBone(public int iSequence, public int iLocalBone) const;  // maps local sequence bone to global bone
        //public int RemapAnimBone(public int iAnim, public int iLocalBone) const;     // maps local animations bone to global bone

        public int numbonecontrollers;     // bone controllers
        public int bonecontrollerindex;
        //inline mstudiobonecontroller_t * pBonecontroller(public int i) const { Assert(i >= 0 && i<numbonecontrollers); return (mstudiobonecontroller_t*) (((byte*)this) + bonecontrollerindex) + i; };

        public int numhitboxsets;
        public int hitboxsetindex;

        // Look up hitbox set by index
        //   mstudiohitboxset_t* pHitboxSet(public int i) const
        //{ 
        //	Assert(i >= 0 && i<numhitboxsets); 
        //	return (mstudiohitboxset_t*) (((byte*)this) + hitboxsetindex ) + i;
        //   };

        // Calls through to hitbox to determine size of specified set
        //inline mstudiobbox_t * pHitbox(public int i, public int set) const
        //	{ 
        //		mstudiohitboxset_t const * s = pHitboxSet(set);
        //		if ( !s )
        //			return NULL;

        //		return s->pHitbox(i );
        //};

        // Calls through to set to get hitbox count for set
        //inline public int iHitboxCount(public int set) const
        //{
        //	mstudiohitboxset_t const * s = pHitboxSet(set);
        //	if ( !s )
        //		return 0;

        //	return s->numhitboxes;
        //};

        // file local animations? and sequences
        //private:
        public int numlocalanim;           // animations/poses
        public int localanimindex;     // animation descriptions
                                //inline mstudioanimdesc_t * pLocalAnimdesc(public int i) const { if (i< 0 || i >= numlocalanim) i = 0; return (mstudioanimdesc_t*) (((byte*)this) + localanimindex) + i; };

        public int numlocalseq;                // sequences
        public int localseqindex;
        //inline mstudioseqdesc_t * pLocalSeqdesc(public int i) const { if (i< 0 || i >= numlocalseq) i = 0; return (mstudioseqdesc_t*) (((byte*)this) + localseqindex) + i; };

        //public:
        //bool SequencesAvailable() const;
        //public int GetNumSeq() const;
        //mstudioanimdesc_t	&pAnimdesc(public int i) const;
        //mstudioseqdesc_t	&pSeqdesc(public int i) const;
        //public int iRelativeAnim(public int baseseq, public int relanim) const;  // maps seq local anim reference to global anim index
        //public int iRelativeSeq(public int baseseq, public int relseq) const;        // maps seq local seq reference to global seq index

        //private:
        public int activitylistversion;    // initialization flag - have the sequences been indexed?
        public int eventsindexed;
        //public:
        //public int GetSequenceActivity(public int iSequence);
        //void SetSequenceActivity(public int iSequence, public int iActivity);
        //public int GetActivityListVersion(void );
        //void SetActivityListVersion(public int version) const;
        //public int GetEventListVersion(void );
        //void SetEventListVersion(public int version);

        // raw textures
        public int numtextures;
        public int textureindex;
        //inline mstudiotexture_t * pTexture(public int i) const { Assert(i >= 0 && i<numtextures ); return (mstudiotexture_t*) (((byte*)this) + textureindex) + i; };


        // raw textures search paths
        public int numcdtextures;
        public int cdtextureindex;
        //inline char* pCdtexture(public int i) const { return (((char*)this) + *((public int *)(((byte *)this) + cdtextureindex) + i)); };

        // replaceable textures tables
        public int numskinref;
        public int numskinfamilies;
        public int skinindex;
        //inline short* pSkinref(public int i) const { return (short*) (((byte*)this) + skinindex) + i; };

        public int numbodyparts;
        public int bodypartindex;
        //inline mstudiobodyparts_t   * pBodypart(public int i) const { return (mstudiobodyparts_t*) (((byte*)this) + bodypartindex) + i; };

        // queryable attachable popublic ints
        //private:
        public int numlocalattachments;
        public int localattachmentindex;
        //inline mstudioattachment_t  * pLocalAttachment(public int i) const { Assert(i >= 0 && i<numlocalattachments); return (mstudioattachment_t*) (((byte*)this) + localattachmentindex) + i; };
        //public:
        //public int GetNumAttachments(void ) const;
        //const mstudioattachment_t &pAttachment(public int i) const;
        //public int GetAttachmentBone(public int i);
        //// used on my tools in hlmv, not persistant
        //void SetAttachmentBone(public int iAttachment, public int iBone);

        // animation node to animation node transition graph
        //private:
        public int numlocalnodes;
        public int localnodeindex;
        public int localnodenameindex;
        //inline char* pszLocalNodeName(public int iNode) const { Assert(iNode >= 0 && iNode<numlocalnodes); return (((char*)this) + *((public int *)(((byte *)this) + localnodenameindex) + iNode)); }
        //inline byte* pLocalTransition(public int i) const { Assert(i >= 0 && i<(numlocalnodes* numlocalnodes)); return (byte*) (((byte*)this) + localnodeindex) + i; };

        //public:
        //public int EntryNode(public int iSequence);
        //public int ExitNode(public int iSequence);
        //char* pszNodeName(public int iNode);
        //public int GetTransition(public int iFrom, public int iTo) const;

        public int numflexdesc;
        public int flexdescindex;
        //inline mstudioflexdesc_t * pFlexdesc(public int i) const { Assert(i >= 0 && i<numflexdesc); return (mstudioflexdesc_t*) (((byte*)this) + flexdescindex) + i; };

        public int numflexcontrollers;
        public int flexcontrollerindex;
        //inline mstudioflexcontroller_t * pFlexcontroller(LocalFlexController_t i) const { Assert(numflexcontrollers == 0 || (i >= 0 && i<numflexcontrollers ) ); return (mstudioflexcontroller_t*) (((byte*)this) + flexcontrollerindex) + i; };

        public int numflexrules;
        public int flexruleindex;
        //inline mstudioflexrule_t * pFlexRule(public int i) const { Assert(i >= 0 && i<numflexrules); return (mstudioflexrule_t*) (((byte*)this) + flexruleindex) + i; };

        public int numikchains;
        public int ikchainindex;
        //inline mstudioikchain_t * pIKChain(public int i) const { Assert(i >= 0 && i<numikchains); return (mstudioikchain_t*) (((byte*)this) + ikchainindex) + i; };

        public int nummouths;
        public int mouthindex;
        //inline mstudiomouth_t * pMouth(public int i) const { Assert(i >= 0 && i<nummouths); return (mstudiomouth_t*) (((byte*)this) + mouthindex) + i; };

        //private:
        public int numlocalposeparameters;
        public int localposeparamindex;
        //inline mstudioposeparamdesc_t * pLocalPoseParameter(public int i) const { Assert(i >= 0 && i<numlocalposeparameters); return (mstudioposeparamdesc_t*) (((byte*)this) + localposeparamindex) + i; };
        //public:
        //public int GetNumPoseParameters(void ) const;
        //const mstudioposeparamdesc_t &pPoseParameter(public int i);
        //public int GetSharedPoseParameter(public int iSequence, public int iLocalPose) const;

        public int surfacepropindex;
        //inline char* const pszSurfaceProp( void ) const { return ((char*)this) + surfacepropindex; }

        // Key values
        public int keyvalueindex;
        public int keyvaluesize;
        //inline const char* KeyValueText( void ) const { return keyvaluesize != 0 ? ((char*)this) + keyvalueindex : NULL; }

        public int numlocalikautoplaylocks;
        public int localikautoplaylockindex;
        //inline mstudioiklock_t * pLocalIKAutoplayLock(public int i) const { Assert(i >= 0 && i<numlocalikautoplaylocks); return (mstudioiklock_t*) (((byte*)this) + localikautoplaylockindex) + i; };

        //public int GetNumIKAutoplayLocks(void ) const;
        //const mstudioiklock_t &pIKAutoplayLock(public int i);
        //public int CountAutoplaySequences() const;
        //public int CopyAutoplaySequences(unsigned short* pOut, public int outCount) const;
        //public int GetAutoplayList(unsigned short** pOut) const;

        // The collision model mass that jay wanted
        public float mass;
        public int contents;

        // external animations, models, etc.
        public int numincludemodels;
        public int includemodelindex;
        //inline mstudiomodelgroup_t * pModelGroup(public int i) const { Assert(i >= 0 && i<numincludemodels); return (mstudiomodelgroup_t*) (((byte*)this) + includemodelindex) + i; };
        // implementation specific call to get a named model
        //const studiohdr_t* FindModel( void **cache, char const *modelname ) const;

        // implementation specific back popublic inter to virtual data
        public int virtualModel;
        //virtualmodel_t* GetVirtualModel(void ) const;

        // for demand loaded animation blocks
        public int szanimblocknameindex;
        //inline char* const pszAnimBlockName( void ) const { return ((char*)this) + szanimblocknameindex; }
        public int numanimblocks;
        public int animblockindex;
        //inline mstudioanimblock_t * pAnimBlock(public int i) const { Assert(i > 0 && i<numanimblocks); return (mstudioanimblock_t*) (((byte*)this) + animblockindex) + i; };
        public int animblockModel;
        //byte* GetAnimBlock(public int i) const;

        public int bonetablebynameindex;
        //inline const byte* GetBoneTableSortedByName() const { return (byte*) this + bonetablebynameindex; }

        // used by tools only that don't cache, but persist mdl's peer data
        // engine uses virtualModel to back link to cache popublic inters
        public int pVertexBase;
        public int pIndexBase;

        // if STUDIOHDR_FLAGS_CONSTANT_DIRECTIONAL_LIGHT_DOT is set,
        // this value is used to calculate directional components of lighting 
        // on static props
        byte constdirectionallightdot;

        // set during load of mdl data to track *desired* lod configuration (not actual)
        // the *actual* clamped root lod is found in studiohwdata
        // this is stored here as a global store to ensure the staged loading matches the rendering
        byte rootLOD;

        // set in the mdl data to specify that lod configuration should only allow first numAllowRootLODs
        // to be set as root LOD:
        //	numAllowedRootLODs = 0	means no restriction, any lod can be set as root lod.
        //	numAllowedRootLODs = N	means that lod0 - lod(N-1) can be set as root lod, but not lodN or lower.
        byte numAllowedRootLODs;

        byte unused;

        public int unused4; // zero out if version < 47

        public int numflexcontrollerui;
        public int flexcontrolleruiindex;
        //mstudioflexcontrollerui_t* pFlexControllerUI(public int i) const { Assert(i >= 0 && i<numflexcontrollerui); return (mstudioflexcontrollerui_t*) (((byte*)this) + flexcontrolleruiindex) + i; }

        public float flVertAnimFixedPointScale;
        //inline public float VertAnimFixedPopublic intScale() const { return (flags & STUDIOHDR_FLAGS_VERT_ANIM_FIXED_POpublic int_SCALE ) ? flVertAnimFixedPopublic intScale : 1.0f / 4096.0f; }

        public int unused3;

        // FIXME: Remove when we up the model version. Move all fields of studiohdr2_t public into studiohdr_t.
        public int studiohdr2index;
        //studiohdr2_t* pStudioHdr2() const { return (studiohdr2_t*) (((byte*)this ) + studiohdr2index ); }

        // Src bone transforms are transformations that will convert .dmx or .smd-based animations public into .mdl-based animations
        //public int NumSrcBoneTransforms() const { return studiohdr2index? pStudioHdr2()->numsrcbonetransform : 0; }
        //	const mstudiosrcbonetransform_t* SrcBoneTransform( public int i ) const { Assert(i >= 0 && i<NumSrcBoneTransforms()); return (mstudiosrcbonetransform_t*) (((byte*)this) + pStudioHdr2()->srcbonetransformindex) + i; }

        //inline public int IllumPositionAttachmentIndex() const { return studiohdr2index? pStudioHdr2()->IllumPositionAttachmentIndex() : 0; }

        //inline public float MaxEyeDeflection() const { return studiohdr2index? pStudioHdr2()->MaxEyeDeflection() : 0.866f; } // default to cos(30) if not set

        //inline mstudiolinearbone_t * pLinearBones() const { return studiohdr2index? pStudioHdr2()->pLinearBones() : NULL; }

        //inline public int BoneFlexDriverCount() const { return studiohdr2index? pStudioHdr2()->m_nBoneFlexDriverCount : 0; }
        //inline const mstudioboneflexdriver_t* BoneFlexDriver( public int i ) const { Assert(i >= 0 && i<BoneFlexDriverCount() ); return studiohdr2index? pStudioHdr2()->pBoneFlexDriver(i ) : NULL; }

        // NOTE: No room to add stuff? Up the .mdl file format version 
        // [and move all fields in studiohdr2_t public into studiohdr_t and kill studiohdr2_t],
        // or add your stuff to studiohdr2_t. See NumSrcBoneTransforms/SrcBoneTransform for the pattern to use.

        public int unused2;
    }
}
